﻿using NDK.Query.Attributes;
using NDK.Query.Interfaces;
using NDK.Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Query
{
    public class QueryBuilder
    {
        private NdkDbType _dbType;

        public QueryBuilder(NdkDbType dbType)
        {
            _dbType = dbType;
        }



        public string GetInsertSql<T>(T model, ModelConfig? config = null) where T : IQueryBuilderable
        {
            StringBuilder sb = new StringBuilder();

            if (config is null)
            {

                DbTable? dbTable = model.GetType().GetCustomAttribute<DbTable>(true);
                if (dbTable == null)
                {
                    throw new Exception("DbTable attribute wasn't provided to the class.");
                }

                sb.AppendLine($"INSERT INTO {dbTable.Table}");

                sb.AppendLine("(");

                PropertyInfo[] properties = model.GetType().GetProperties();

                for (int i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];
                    AutoGenerated? AutoGenerated = prop.GetCustomAttribute<AutoGenerated>(true);

                    if (AutoGenerated is null)
                    {
                        ForeignKey? foreignKey= prop.GetCustomAttribute<ForeignKey>(true);

                        if (foreignKey is null)
                        {
                            DbColumn? dbColumn = prop.GetCustomAttribute<DbColumn>(true);
                            if (dbColumn is null)
                            {
                                sb.AppendLine($"{prop.Name}{(i < properties.Length - 1 ? "," : "")}");
                            }
                            else
                            {
                                sb.AppendLine($"{dbColumn.Name}{(i < properties.Length - 1 ? "," : "")}");
                            }
                        }
                        else
                        {

                        }


                        
                    }
                }

                sb.AppendLine(")");

                sb.AppendLine("VALUES(");

                for (int i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];
                    AutoGenerated? AutoGenerated = prop.GetCustomAttribute<AutoGenerated>(true);

                    if (AutoGenerated is null)
                    {
                        sb.AppendLine($"@{prop.Name}{(i < properties.Length - 1 ? "," : "")}");
                    }
                }

                sb.AppendLine(")");



            }
            else
            {

            }







            return sb.ToString();
        }



    }

}
