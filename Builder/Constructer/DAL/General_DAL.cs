/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       General_DAL.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 13:08:53
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DBUtility.DBHelperSQL;

namespace Builder.Constructer.DAL
{
    public class General_DAL
    {
        private StringBuilder contents = new StringBuilder(100);

        private readonly string FILE_PATH = ConfigurationManager.AppSettings["DALFilePath"];

        private readonly DataSet ds;

        public General_DAL()
        {
            //  获取所有表的结构
            ds = GetAllTablesStruct();
        }

        /// <summary>
        /// DAL 建造
        /// </summary>
        /// <returns></returns>
        public StringBuilder Construct()
        {

            foreach (DataTable dt in ds.Tables)
            {
                contents.Append(SetHead(dt));

                contents.Append(SetPrivateField(dt));
                contents.Append(SetGetModel(dt));

                contents.Append(SetFoot(dt));

                Out_File(dt.TableName, contents.ToString());
                contents.Clear();
            }

            return null;
        }

        private string SetHead(DataTable dt)
        {
            string head = $@"using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DBUtility;

namespace DAL.Table
{{
    public class {dt.TableName} : IDAL<Models.{dt.TableName}>
    {{
";
            return head;
        }

        private string SetPrivateField(DataTable dt)
        {
            string result = "\t\tprivate string field = \" * \"," +
                "\r\n\t\t\t\t\t\ttable = \" " + dt.TableName + " \"," +
                        "\r\n\t\t\t\t\t\twhere = \" 1=1 \"," +
                       "\r\n\t\t\t\t\t\torderBy = \" Id \"," +
                        "\r\n\t\t\t\t\t\tgroupBy = \"\";\r\n\r\n";
            return result;
        }

        private string SetGetModel(DataTable dt)
        {
            string result = "";

            result += $"\t\tpublic Models.{dt.TableName} GetModel(int id)\r\n";
            result += "\t\t{\r\n";
            result += "\t\t\tstring queryString = $" + "\"SELECT TOP 1 * FROM " + dt.TableName + " WHERE Id = {id}\";\r\n";
            result += "\t\t\tDataTable dt = DbHelperSQL.Query(queryString).Tables[0];\r\n";
            result += "\t\t\treturn DataTableToList(dt).FirstOrDefault();\r\n";
            result += "\t\t}\r\n\r\n";

            return result;
        }

        private string SetFoot(DataTable dt)
        {
            string foot = @"
    }
}
";
            return foot;
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
