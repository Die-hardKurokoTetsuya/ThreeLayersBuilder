/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       General_Model.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:49:35
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DBUtility.DBHelperSQL;

namespace Builder.Constructer.Models
{
    public class General_Model : IConstructible
    {
        private readonly DataSet ds;
        private StringBuilder contents;

        public General_Model()
        {
            //  获取所有表的结构
            ds = GetAllTablesStruct();
        }

        /// <summary>
        /// Model 建造
        /// </summary>
        /// <returns></returns>
        public StringBuilder Construct()
        {
            Fill_Model(ds.Tables[0]);



            return null;
        }

        private void Fill_Model(DataTable dt)
        {
            contents = new StringBuilder(200);

                contents.Append(
                    $@"using System;

namespace Models
{{
    public class {dt.TableName} : ICloneable
    {{
        {Get_Field(dt)}
    }}
}}");
            
        }

        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string Get_Field(DataTable dt)
        {
            StringBuilder field = new StringBuilder(100);

            foreach (DataRow row in dt.Rows)
            {
                string typeName = row["TYPE_NAME"].ToString();
                bool nullable = Convert.ToBoolean(row["NULLABLE"]);

                field.Append($@"      public {Get_Type(typeName, nullable)} {row["COLUMN_NAME"]} {{ get; set; }}
");

            }
            return field.ToString();
        }

        private string Get_Type(string type, bool isNull)
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

        private string GetFunction(DataTable dt)
        {
            StringBuilder function = new StringBuilder(100);



            return function.ToString();
        }
    }
}
