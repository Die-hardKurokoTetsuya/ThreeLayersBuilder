/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       CreateFactoryFile.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-24 10:33:55
 *
 *  Version:        V1.0.0
 ***********************************************/

using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using static DBUtility.DBHelperSQL;

namespace Builder.Constructer.DAL
{
    public class CreateFactoryFile
    {
        private StringBuilder contents = new StringBuilder(500);

        private readonly string FILE_PATH = ConfigurationManager.AppSettings["DALFilePath"];

        private readonly DataSet ds;
        private string nameSpace = "";

        public CreateFactoryFile(string nameSpace)
        {
            this.nameSpace = nameSpace + ".";
            //  获取所有表的结构
            ds = GetAllTablesStruct();
        }

        public void Construct()
        {
            contents.Append(SetHeader());
            contents.Append(SetTableEnum(ds.Tables[0]));
            contents.Append(SetNew());
            contents.Append(SetFooter());

            Out_File(contents.ToString());
        }

        private string SetHeader()
        {
            StringBuilder result = new StringBuilder(100);

            result.Append(@"using System;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using static Common.Loger;
");
            result.Append($"namespace {nameSpace}DAL\r\n");
            result.Append($"{{\r\n");
            result.Append($"\tpublic class Factory\r\n");
            result.Append($"\t{{\r\n");
            result.Append($"\t\tprivate static readonly object lockObj = new object();\r\n\r\n");

            return result.ToString();
        }

        private string SetTableEnum(DataTable dt)
        {
            StringBuilder result = new StringBuilder(100);

            result.Append("\t\tpublic enum TableEnum\r\n");
            result.Append("\t\t{\r\n");
            foreach (DataRow row in dt.Rows)
            {
                result.Append($"\t\t\t{row["COLUMN_NAME"]},\r\n");
            }

            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetNew()
        {
            StringBuilder result = new StringBuilder(100);

            result.Append("\t\tpublic static IDAL<T> New<T>(TableEnum table)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\ttry\r\n");
            result.Append("\t\t\t{\r\n");
            result.Append("\t\t\t\tstring tableName = table.ToString();\r\n");
            result.Append("\t\t\t\tCache cache = HttpRuntime.Cache;\r\n\r\n");
            result.Append("\t\t\t\tIDAL<T> _dal = cache.Get(tableName) as IDAL<T>;\r\n\r\n");
            result.Append("\t\t\t\tif (_dal == null)\r\n");
            result.Append("\t\t\t\t{\r\n");
            result.Append("\t\t\t\t\tlock (lockObj)\r\n");
            result.Append("\t\t\t\t\t{\r\n");
            result.Append("\t\t\t\t\t\tif (_dal == null)\r\n");
            result.Append("\t\t\t\t\t\t{\r\n");
            result.Append("\t\t\t\t\t\t\t_dal = (IDAL<T>)Assembly.Load(\"Per.DAL\").CreateInstance($\"Per.DAL.Table.{tableName}\");\r\n");
            result.Append("\t\t\t\t\t\t\tcache.Insert(tableName, _dal);\r\n");
            result.Append("\t\t\t\t\t\t}\r\n");
            result.Append("\t\t\t\t\t}\r\n");
            result.Append("\t\t\t\t}\r\n\r\n");
            result.Append("\t\t\t\t\treturn _dal;\r\n");
            result.Append("\t\t\t}\r\n");
            result.Append("\t\t\tcatch (Exception ex)\r\n");
            result.Append("\t\t\t{\r\n");
            result.Append("\t\t\t\tWriteLog($\"{ex.Message} \\r\\n {ex.StackTrace}\", LogType.Error);\r\n");
            result.Append("\t\t\t\treturn null;\r\n");
            result.Append("\t\t\t}\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetFooter()
        {
            string footer = @"    }
}
";
            return footer;
        }

        private void Out_File(string contents)
        {
            string filePath = FILE_PATH + "Factory.cs";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(FILE_PATH);
            }
            File.WriteAllText(filePath, contents);
        }
    }
}
