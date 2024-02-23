using Dapper;
using NDK.Core.Models;
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


        private static DynamicParameters GetParameters(this NDKRequest request, NDKDbConnectionConfiguration configuration)
        {
            var parameters = new DynamicParameters();

            string alias = configuration.GetParamSymbol();

            if (request.Paging is not null)
            {
                parameters.Add(nameof(request.Paging.ItemsPerPage), request.Paging.ItemsPerPage);
                parameters.Add(nameof(request.Paging.Page), request.Paging.Page);
            }

            if (request.FiltersGroups.Any())
            {
                foreach (var fg in request.FiltersGroups)
                {
                    parameters.AddDynamicParams(GetParametersByFilterGroup(fg, alias));
                }
            }

            return parameters;
        }

        private static DynamicParameters GetParametersByFilterGroup(NDKFilterGroup group, string alias)
        {
            var parameters = new DynamicParameters();

            foreach (var f in group.Filters)
            {
                parameters.Add($"{alias}{f.PropertyName}{f.Id}", f.Value);

                if (!string.IsNullOrWhiteSpace(f.PropertyName2))
                {
                    parameters.Add($"{alias}{f.PropertyName2}{f.Id}", f.Value2);
                }
            }

            if (group.InternalGroups.Any())
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

            if (request.FiltersGroups.Any())
            {
                if (configuration.Type == NDKDbType.ORACLE)
                {
                    if (request.FiltersGroups.Any())
                    {
                        StringBuilder where = new StringBuilder();


                        where.AppendLine(" WHERE ");

                        for (int i = 0; i < request.FiltersGroups.Count; i++)
                        {
                            where.AppendLine(GetFilterGroup(request.FiltersGroups[i], i == request.FiltersGroups.Count - 1, configuration).ToString());

                        }


                        sb.Replace("<<where>>", where.ToString());
                    }
                }
                else
                {
                    sb.AppendLine(" WHERE ");

                    for (int i = 0; i < request.FiltersGroups.Count; i++)
                    {
                        sb.AppendLine(GetFilterGroup(request.FiltersGroups[i], i == request.FiltersGroups.Count - 1, configuration).ToString());

                    }
                }
            }

            if (request.Paging is not null)
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

            for (int i = 0; i < filterGroup.Filters.Count; i++)
            {
                sb.AppendLine(GetFilter(filterGroup.Filters[i], i == filterGroup.Filters.Count - 1, configuration).ToString());
            }

            if (filterGroup.InternalGroups is not null)
            {
                for (int i = 0; i <= filterGroup.InternalGroups.Count; i++)
                {
                    sb.AppendLine(GetFilterGroup(filterGroup.InternalGroups[i], i == filterGroup.Filters.Count - 1, configuration).ToString());
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

            if (filter.NDKOperatorType.Equals(NDKOperatorType.BETWEEN))
            {
                sb.Append($"({alias}{filter.PropertyName}{filter.Id} AND {alias}{filter.PropertyName2}{filter.Id}) ");
            }
            else
            {
                sb.Append($"({alias}{filter.PropertyName}{filter.Id}) ");
            }

            if (!isLastOne)
            {
                sb.AppendLine($" {filter.NDKConditionType.ToString()} ");
            }


            return sb;
        }

        private static string GetOperator(NDKFilter filter, NDKDbConnectionConfiguration configuration)
        {
            return filter.NDKOperatorType switch
            {
                NDKOperatorType.EQUAL => "=",
                NDKOperatorType.NOTEQUAL => "<>",

                NDKOperatorType.LESSTHAN => "<",
                NDKOperatorType.LESSTHANOREQUAL => "<=",

                NDKOperatorType.GREATERTHAN => ">",
                NDKOperatorType.GREATERTHANOREQUAL => ">=",

                NDKOperatorType.IN => "IN",
                NDKOperatorType.BETWEEN => "BETWEEN",

                _ => "="
            };
        }
    }
}
