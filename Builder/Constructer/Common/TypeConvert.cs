/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       TypeConvert.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-17 09:07:21
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;

namespace Builder.Constructer.Common
{
    public static class TypeConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isNull"></param>
        /// <returns></returns>
        public static string SqlDBTypeToCSharpType(string type, bool isNull)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("数据类型不能为空");
            string sqlType = type.Trim().Split(' ')[0];

            if (isNull)
            {
                switch (sqlType.ToLower())
                {
                    case "bit": return "bool?";
                    case "int":
                    case "tinyint":
                    case "smallint":
                        return "int?";
                    case "bigint": return "long?";
                    case "smallmoney":
                    case "money":
                    case "numeric":
                    case "decimal":
                        return "decimal?";
                    case "float": return "double?";
                    case "real": return "float?";
                    case "smalldatetime":
                    case "datetime":
                    case "timestamp":
                        return "DataTime?";
                    case "char":
                    case "text":
                    case "varchar":
                    case "nchar":
                    case "ntext":
                    case "nvarchar":
                        return "string";
                    case "binary":
                    case "varbinary":
                    case "image":
                        return "byte[]";
                    case "uniqueidentifier": return "Guid";
                    case "Variant": return "object";
                    default:
                        throw new Exception("未知的数据类型");
                }
            }
            else
            {
                switch (sqlType.ToLower())
                {
                    case "bit": return "bool";
                    case "int":
                    case "tinyint":
                    case "smallint":
                    case "bigint":
                        return "int";
                    case "smallmoney":
                    case "money":
                    case "numeric":
                    case "decimal":
                        return "decimal";
                    case "float": return "double";
                    case "real": return "single";
                    case "smalldatetime":
                    case "datetime":
                    case "timestamp":
                        return "DataTime";
                    case "char":
                    case "text":
                    case "varchar":
                    case "nchar":
                    case "ntext":
                    case "nvarchar":
                        return "string";
                    case "binary":
                    case "varbinary":
                    case "image":
                        return "byte[]";
                    case "uniqueidentifier": return "Guid";
                    case "Variant": return "object";
                    default:
                        throw new Exception("未知的数据类型");
                }
            }
        }
    }
}
