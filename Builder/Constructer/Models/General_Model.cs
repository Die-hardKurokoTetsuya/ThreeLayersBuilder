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

using Builder.Constructer.Common;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using static DBUtility.DBHelperSQL;

namespace Builder.Constructer.Models
{
    public class General_Model
    {
        private readonly string FILE_PATH = ConfigurationManager.AppSettings["ModelsFilePath"];

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

            foreach (DataTable dt in ds.Tables)
            {
                Fill_Model(dt);
            }

            return contents;
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

{GetFunction(dt)}
    }}
}}
");

            Out_File(dt.TableName, contents.ToString());
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

                field.Append($@"        public {TypeConvert.SqlDBTypeToCSharpType(typeName, nullable)} {row["COLUMN_NAME"]} {{ get; set; }}
");

            }
            return field.ToString();
        }

        private string GetFunction(DataTable dt)
        {
            StringBuilder function = new StringBuilder(100);

            function.Append($@"        public object Clone()
        {{
            {dt.TableName} model = new {dt.TableName}
            {{
{GetFuncField(dt)}
            }};
            return model;
        }}

        public {dt.TableName} DeepClone()
        {{
            return Clone() as {dt.TableName};
        }}");

            return function.ToString();
        }

        private string GetFuncField(DataTable dt)
        {
            StringBuilder funcField = new StringBuilder(50);
            foreach (DataRow row in dt.Rows)
            {
                funcField.Append($@"                {row["COLUMN_NAME"]} = {row["COLUMN_NAME"]},
");
            }
            return funcField.Remove(funcField.Length - 1, 1).ToString();
        }

        private void Out_File(string name, string contents)
        {
            string filePath = FILE_PATH + name + ".cs";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(FILE_PATH);
            }
            File.WriteAllText(filePath, contents);
        }
    }
}
