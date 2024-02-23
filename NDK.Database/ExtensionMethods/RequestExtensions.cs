using Dapper;
using NDK.Core.Models;
using NDK.Core.Extensions;
using NDK.Database.ExtensionMethods.Internal;
using NDK.Database.Models;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.ExtensionMethods
{
    public static class RequestExtensions
    {

        public static (string query, DynamicParameters parameters) GetRequestData(this NDKRequest request, string query, NDKDbConnectionConfiguration configuration)
        {
            return (request.GetRequestSql(query, configuration).ToString(),
                    request.GetParameters(configuration));
        }


        private static DynamicParameters GetParameters(this NDKRequest request, NDKDbConnectionConfiguration configuration, bool childGroup = false)
        {
            var parameters = new DynamicParameters();

            string alias = configuration.GetParamSymbol();

            if (request.Pager is not null)
            {
                parameters.Add(nameof(request.Pager.ItemsPerPage), request.Pager.ItemsPerPage);
                parameters.Add(nameof(request.Pager.Page), request.Pager.Page);
            }

            if (request?.FilterStructure?.FilterGroups.Count >= 0)
            {
                foreach (var fg in request.FilterStructure.FilterGroups)
                {
                    parameters.AddDynamicParams(GetParametersByFilterGroup(fg, alias));

                    if (fg.InternalGroups.HasAny())
                    {
                        foreach(var ig in fg.InternalGroups)
                        {
                            parameters.AddDynamicParams(GetParametersByFilterGroup(ig, alias));
                        }
                    }
                }
            }

            return parameters;
        }

        private static DynamicParameters GetParametersByFilterGroup(NDKFilterGroup group, string alias)
        {
            var parameters = new DynamicParameters();

            foreach (var f in group.Filters)
            {
                f.Value = f.OperatorType switch
                {
                    NDKOperatorType.STARTSWITH => $"{f.Value}%",
                    NDKOperatorType.ENDSWITH=> $"%{f.Value}",
                    NDKOperatorType.CONTAINS=> $"%{f.Value}%",
                    _ => f.Value
                };
                
                parameters.Add($"{alias}{f.PropertyName}{f.Id}", f.Value);

                if (!string.IsNullOrWhiteSpace(f.PropertyName2))
                {
                    parameters.Add($"{alias}{f.PropertyName2}{f.Id}", f.Value2);
                }
            }

            if (group.InternalGroups.HasAny())
            {
                foreach (var g in group.InternalGroups)
                {
                    parameters.AddDynamicParams(GetParametersByFilterGroup(g, alias));
                }
            }

            return parameters;
        }

        private static StringBuilder GetRequestSql(this NDKRequest request, string query, NDKDbConnectionConfiguration configuration)
        {
            StringBuilder sb;

            sb = configuration.Type switch
            {
                NDKDbType.ORACLE => new StringBuilder($"SELECT * FROM ( {query} <<where>> )"),
                _ => new StringBuilder(query)
            };

            if (request?.FilterStructure?.FilterGroups.HasAny() ?? false)
            {
                if (configuration.Type == NDKDbType.ORACLE)
                {
                    if (request.FilterStructure.FilterGroups.HasAny())
                    {
                        StringBuilder where = new StringBuilder();


                        where.AppendLine(" WHERE ");

                        for (int i = 0; i < request.FilterStructure.FilterGroups.Count; i++)
                        {
                            where.AppendLine(GetFilterGroup(request.FilterStructure.FilterGroups.ElementAt(i), i == request.FilterStructure.FilterGroups.Count - 1, configuration).ToString());

                        }


                        sb.Replace("<<where>>", where.ToString());
                    }
                }
                else
                {
                    sb.AppendLine(" WHERE ");

                    for (int i = 0; i < request.FilterStructure.FilterGroups.Count; i++)
                    {
                        sb.AppendLine(GetFilterGroup(request.FilterStructure.FilterGroups.ElementAt(i), i == request.FilterStructure.FilterGroups.Count - 1, configuration).ToString());

                    }
                }
            }

            if (request?.Pager is not null)
            {
                sb.AppendLine(configuration.Type switch
                {
                    NDKDbType.ORACLE => " WHERE rownum < ((:Page * :ItemsPerPage) + 1 ) ",
                    NDKDbType.MYSQL => " LIMIT (@Page * @ItemsPerPage),@ItemsPerPage ",
                    NDKDbType.SQLSERVER => " OFFSET @ItemsPerPage ROWS FETCH NEXT (@Page * @ItemsPerPage) ",
                    _ => throw new NotSupportedException()
                }); ;

            }

            return sb;
        }

        private static StringBuilder GetFilterGroup(NDKFilterGroup filterGroup, bool isLastOne, NDKDbConnectionConfiguration configuration)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" ( ");

            foreach(var item in filterGroup.OrderList)
            {
                if (item.Value is null) continue;

                if (item.Type == NDKFilterGroup.IdentifierType.Filter)
                {
                    sb.AppendLine(GetFilter((NDKFilter) item.Value, item.IsLastOne, configuration).ToString());
                }
                else if (item.Type == NDKFilterGroup.IdentifierType.FilterGroup)
                {
                    sb.AppendLine(GetFilterGroup((NDKFilterGroup) item.Value, item.IsLastOne, configuration).ToString());
                }
            }

            if (!isLastOne)
            {
                sb.AppendLine($" {filterGroup.ConditionType.ToString()} ");
            }

            sb.AppendLine(" ) ");

            return sb;
        }

        private static StringBuilder GetFilter(NDKFilter filter, bool isLastOne, NDKDbConnectionConfiguration configuration)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{filter.Target} {GetOperator(filter, configuration)} ");

            string alias = configuration.Type switch
            {
                NDKDbType.ORACLE => ":",
                _ => "@"
            };

            if (filter.OperatorType.Equals(NDKOperatorType.BETWEEN))
            {
                sb.Append($"({alias}{filter.PropertyName}{filter.Id} AND {alias}{filter.PropertyName2}{filter.Id}) ");
            }
            else
            {
                sb.Append($"({alias}{filter.PropertyName}{filter.Id}) ");
            }

            if (!isLastOne)
            {
                sb.AppendLine($" {filter.ConditionType.ToString()} ");
            }


            return sb;
        }

        private static string GetOperator(NDKFilter filter, NDKDbConnectionConfiguration configuration)
        {
            return filter.OperatorType switch
            {
                NDKOperatorType.EQUAL => "=",
                NDKOperatorType.NOTEQUAL => "<>",

                NDKOperatorType.LESSTHAN => "<",
                NDKOperatorType.LESSTHANOREQUAL => "<=",

                NDKOperatorType.GREATERTHAN => ">",
                NDKOperatorType.GREATERTHANOREQUAL => ">=",

                NDKOperatorType.IN => "IN",
                NDKOperatorType.BETWEEN => "BETWEEN",

                NDKOperatorType.STARTSWITH or
                NDKOperatorType.ENDSWITH or 
                NDKOperatorType.CONTAINS =>
                    "LIKE",

                _ => "="
            };
        }
    }
}
