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
                        return "DateTime?";
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
                        throw new Exception("未知的数据类型 sqlType: " + sqlType.ToLower());
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
                        return "int";
                    case "bigint": return "long";
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
                        return "DateTime";
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


        public static string ToSqlDbType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("数据类型不能为空");
            string sqlType = type.Trim().Split(' ')[0];

            switch (sqlType.ToLower())
            {
                case "bit": return "Bit";
                case "int": return "Int";
                case "tinyint": return "TinyInt";
                case "smallint": return "SmallInt";
                case "bigint": return "BigInt";
                case "smallmoney": return "SmallMoney";
                case "money": return "Money";
                case "decimal": return "Decimal";
                case "float": return "Float";
                case "real": return "Real";
                case "smalldatetime": return "SmallDateTime";
                case "datetime": return "DateTime";
                case "timestamp": return "Timestamp";
                case "char": return "Char";
                case "text": return "Text";
                case "varchar": return "VarChar";
                case "nchar": return "NChar";
                case "ntext": return "NText";
                case "nvarchar": return "NVarChar";
                case "binary": return "Binary";
                case "varbinary": return "VarBinary";
                case "image": return "Image";
                case "uniqueidentifier": return "UniqueIdentifier";
                case "Variant": return "Variant";
                default:
                    throw new Exception("未知的数据类型");
            }
        }

        public static string ToConvertType(string type)
        {
            switch (type)
            {
                case "bool?":
                case "bool": return "ToBoolean";
                case "int?":
                case "int": return "ToInt32";
                case "long?":
                case "long": return "ToInt64";
                case "decimal?":
                case "decimal": return "ToDecimal";
                case "double": return "ToDouble";
                case "double?":
                case "float?":
                case "single": return "ToSingle";
                case "DateTime?":
                case "DateTime": return "ToDateTime";
                case "string": return "ToString";
                default:
                    throw new Exception("未知的数据类型 type: " + type);
            }
        }
    }
}
