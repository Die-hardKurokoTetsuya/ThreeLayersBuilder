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
using static Builder.Constructer.Common.TypeConvert;
using static DBUtility.DBHelperSQL;

namespace Builder.Constructer.DAL
{
    public class General_DAL
    {
        private StringBuilder contents = new StringBuilder(500);

        private readonly string FILE_PATH = ConfigurationManager.AppSettings["DALFilePath"];

        private readonly DataSet ds;
        private string nameSpace = "";

        public General_DAL(string nameSpace)
        {
            this.nameSpace = nameSpace + ".";
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
                contents.Append(SetAdd(dt));
                contents.Append(SetDelete(dt));
                contents.Append(SetUpdate(dt));
                contents.Append(SetGetModel(dt));
                contents.Append(SetField(dt));
                contents.Append(SetGroupBy(dt));
                contents.Append(SetOrderBy(dt));
                contents.Append(SetTable(dt));
                contents.Append(SetWhere(dt));
                contents.Append(SetQuery());
                contents.Append(SetGetList(dt));
                contents.Append(SetDataTableToList(dt));
                contents.Append(SetGetPager());
                contents.Append(SetJointFullQueryString());

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
using {nameSpace}DAL;

namespace {nameSpace}DAL.Table
{{
    public class {dt.TableName} : IDAL<{nameSpace}Models.{dt.TableName}>
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

        private string SetAdd(DataTable dt)
        {
            StringBuilder result = new StringBuilder(100);

            result.Append($"\t\tpublic int Add({nameSpace}Models.{dt.TableName} model)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tStringBuilder insertSQL = new StringBuilder(200);\r\n");
            result.Append($"\t\t\tinsertSQL.Append(\"INSERT INTO {dt.TableName}(\");\r\n");

            result.Append($"\t\t\tinsertSQL.Append(\"{string.Join(",", dt.AsEnumerable().Select(t => t["COLUMN_NAME"]))}\");\r\n");
            result.Append($"\t\t\tinsertSQL.Append(\") VALUES(\");\r\n");
            result.Append($"\t\t\tinsertSQL.Append(\"@{string.Join(", @", dt.AsEnumerable().Select(t => t["COLUMN_NAME"]))}\");\r\n");
            result.Append("\t\t\tinsertSQL.Append(\"); select @@IDENTITY\");\r\n");
            result.Append($"\t\t\tSqlParameter[] parameters = {{\r\n");
            foreach (DataRow row in dt.Rows)
            {
                result.Append($"\t\t\t\tnew SqlParameter(\"@{row["COLUMN_NAME"]}\", SqlDbType.{ToSqlDbType(row["TYPE_NAME"].ToString())}, {row["LENGTH"].ToString()}),\r\n");
            }
            result.Append($"\t\t\t}};\r\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Append($"\t\t\tparameters[{i}].Value = model.{dt.Rows[i]["COLUMN_NAME"]};\r\n");
            }
            result.Append("\t\t\tobject obj = DbHelperSQL.GetSingle(insertSQL.ToString(), parameters);\r\n");
            result.Append("\t\t\treturn obj == null ? 0 : Convert.ToInt32(obj);\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetDelete(DataTable dt)
        {
            StringBuilder result = new StringBuilder(100);

            result.Append("\t\tpublic bool Delete(string where)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (!DbHelperSQL.CheckSQLIsSafe(where))\r\n");
            result.Append("\t\t\t\tthrow new Exception(\"query string is unsafe\");\r\n\r\n");
            result.Append("\t\t\tStringBuilder strSql = new StringBuilder(100);\r\n");
            result.Append($"\t\t\tstrSql.Append($\"DELETE FROM {dt.TableName} WHERE {{where}}\");\r\n");
            result.Append("\t\t\tint rows = DbHelperSQL.ExecuteSql(strSql.ToString());\r\n");
            result.Append("\t\t\treturn rows > 0;\r\n");
            result.Append("\t\t}\r\n\r\n");

            IEnumerable<DataRow> list = dt.AsEnumerable().Where(t => t["TYPE_NAME"].ToString().Contains("identity"));

            result.Append("\t\tpublic bool Delete(int id)\r\n");
            result.Append("\t\t{\r\n");
            if (list == null || list.Count() == 0)
                result.Append("\t\t\tthrow new NotImplementedException();\r\n");
            else
            {
                string columnName = list.First()["COLUMN_NAME"].ToString();
                result.Append($"\t\t\tStringBuilder strSql = new StringBuilder(100);\r\n");
                result.Append($"\t\t\tstrSql.Append(\"DELETE FROM {dt.TableName} \");\r\n");
                result.Append($"\t\t\tstrSql.Append(\" where {columnName} = @{columnName}\");\r\n");
                result.Append("\t\t\tSqlParameter[] parameters = {\r\n");
                result.Append($"\t\t\t\tnew SqlParameter(\"@{columnName}\", SqlDbType.{ToSqlDbType(list.First()["TYPE_NAME"].ToString())}, {list.First()["LENGTH"]})\r\n");
                result.Append("\t\t\t};\r\n");
                result.Append($"\t\t\tparameters[0].Value = id;\r\n");
                result.Append("\t\t\tint rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);\r\n");
                result.Append("\t\t\treturn rows > 0;\r\n");
            }
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetUpdate(DataTable dt)
        {
            IEnumerable<DataRow> list = dt.AsEnumerable().Where(t => t["TYPE_NAME"].ToString().Contains("identity"));
            string identityName = list.First()["COLUMN_NAME"].ToString();

            StringBuilder result = new StringBuilder(100);

            result.Append($"\t\tpublic bool Update({nameSpace}Models.{dt.TableName} model)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tStringBuilder strSql = new StringBuilder(50);\r\n");
            result.Append($"\t\t\tstrSql.Append(\"UPDATE {dt.TableName} SET \");\r\n");
            foreach (DataRow row in dt.Rows)
            {
                if (row["COLUMN_NAME"].ToString() == identityName)
                    continue;
                result.Append($"\t\t\tstrSql.Append(\"{row["COLUMN_NAME"]} = @{row["COLUMN_NAME"]}, \");\r\n");
            }
            result = result.Remove(result.Length - 1, 1);
            result.Append($"\t\t\tstrSql.Append(\" WHERE {identityName} = @{identityName}\");\r\n");
            result.Append("\t\t\tSqlParameter[] parameters = {\r\n");
            foreach (DataRow row in dt.Rows)
            {
                if (row["COLUMN_NAME"].ToString() == identityName)
                    continue;
                result.Append($"\t\t\t\tnew SqlParameter(\"@{row["COLUMN_NAME"]}\", SqlDbType.{ToSqlDbType(row["TYPE_NAME"].ToString())}, {row["LENGTH"]}),\r\n");
            }
            result.Append($"\t\t\t\tnew SqlParameter(\"@{identityName}\", SqlDbType.{ToSqlDbType(list.First()["TYPE_NAME"].ToString())}, {list.First()["LENGTH"]})\r\n");
            result.Append("\t\t\t};\r\n");
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["COLUMN_NAME"].ToString() == identityName)
                    continue;
                result.Append($"\t\t\tparameters[{count}].Value = model.{dt.Rows[i]["COLUMN_NAME"]};\r\n");
                count++;
            }
            result.Append($"\t\t\tparameters[{count}].Value = model.{identityName};\r\n");

            result.Append("\t\t\tint rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);\r\n");
            result.Append("\t\t\treturn rows > 0;\r\n");

            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetGetModel(DataTable dt)
        {
            string result = "";

            result += $"\t\tpublic {nameSpace}Models.{dt.TableName} GetModel(int id)\r\n";
            result += "\t\t{\r\n";
            result += "\t\t\tstring queryString = $" + "\"SELECT TOP 1 * FROM " + dt.TableName + " WHERE Id = {id}\";\r\n";
            result += "\t\t\tDataTable dt = DbHelperSQL.Query(queryString).Tables[0];\r\n";
            result += "\t\t\treturn DataTableToList(dt).FirstOrDefault();\r\n";
            result += "\t\t}\r\n\r\n";

            return result;
        }

        private string SetField(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic IDAL<{nameSpace}Models.{dt.TableName}> Field(string field)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (string.IsNullOrWhiteSpace(field))\r\n");
            result.Append("\t\t\t\treturn this;\r\n\r\n");
            result.Append("\t\t\tthis.field = $\" {field} \";\r\n");
            result.Append("\t\t\treturn this;\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetGroupBy(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic IDAL<{nameSpace}Models.{dt.TableName}> GroupBy(string groupBy)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (string.IsNullOrWhiteSpace(groupBy))\r\n");
            result.Append("\t\t\t\treturn this;\r\n\r\n");
            result.Append("\t\t\tthis.groupBy = $\" {groupBy} \";\r\n");
            result.Append("\t\t\treturn this;\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetOrderBy(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic IDAL<{nameSpace}Models.{dt.TableName}> OrderBy(string orderBy)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (string.IsNullOrWhiteSpace(orderBy))\r\n");
            result.Append("\t\t\t\treturn this;\r\n\r\n");
            result.Append("\t\t\tthis.orderBy = $\" {orderBy} \";\r\n");
            result.Append("\t\t\treturn this;\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetTable(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic IDAL<{nameSpace}Models.{dt.TableName}> Table(string table)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (string.IsNullOrWhiteSpace(table))\r\n");
            result.Append("\t\t\t\treturn this;\r\n\r\n");
            result.Append("\t\t\tthis.table = $\" {table} \";\r\n");
            result.Append("\t\t\treturn this;\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetWhere(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic IDAL<{nameSpace}Models.{dt.TableName}> Where(string where)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (string.IsNullOrWhiteSpace(where))\r\n");
            result.Append("\t\t\t\treturn this;\r\n\r\n");
            result.Append("\t\t\tthis.where = $\" {where} \";\r\n");
            result.Append("\t\t\treturn this;\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetQuery()
        {
            StringBuilder result = new StringBuilder(50);

            result.Append("\t\tpublic DataTable Query()\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tif (!DbHelperSQL.CheckSQLIsSafe(ToString()))\r\n");
            result.Append("\t\t\t\tthrow new Exception(\"query string is unsafe\");\r\n");
            result.Append("\t\t\treturn DbHelperSQL.Query(ToString()).Tables[0];\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetGetList(DataTable dt)
        {
            StringBuilder result = new StringBuilder(50);

            result.Append($"\t\tpublic List<{nameSpace}Models.{dt.TableName}> GetList()\r\n");
            result.Append($"\t\t{{\r\n");
            result.Append($"\t\t\tstring queryString = $\"SELECT * FROM {dt.TableName}\";\r\n");
            result.Append($"\t\t\tif (!string.IsNullOrWhiteSpace(where))\r\n");
            result.Append("\t\t\t\tqueryString += $\" WHERE {where}\";\r\n");
            result.Append("\t\t\tif (!string.IsNullOrWhiteSpace(orderBy))\r\n");
            result.Append("\t\t\t\tqueryString += $\" ORDER BY {orderBy}\";\r\n");
            result.Append($"\t\t\tDataTable dt = DbHelperSQL.Query(queryString).Tables[0];\r\n");
            result.Append("\t\t\treturn DataTableToList(dt);\r\n");
            result.Append("\t\t}\r\n");

            return result.ToString();
        }

        private string SetDataTableToList(DataTable dt)
        {
            StringBuilder result = new StringBuilder(300);

            result.Append($"\t\tprivate List<{nameSpace}Models.{dt.TableName}> DataTableToList(DataTable dt)\r\n");
            result.Append($"\t\t{{\r\n");
            result.Append($"\t\t\tList<{nameSpace}Models.{dt.TableName}> list = new List<{nameSpace}Models.{dt.TableName}>(dt.Rows.Count);\r\n\r\n");
            result.Append($"\t\t\tforeach (DataRow row in dt.Rows)\r\n");
            result.Append($"\t\t\t{{\r\n");
            result.Append($"\t\t\t\t{nameSpace}Models.{dt.TableName} model = new {nameSpace}Models.{dt.TableName}();\r\n");
            foreach (DataRow row in dt.Rows)
            {
                string name = row["COLUMN_NAME"].ToString();
                string type = SqlDBTypeToCSharpType(row["TYPE_NAME"].ToString(), Convert.ToBoolean(row["NULLABLE"]));
                type = ToConvertType(type);

                result.Append($"\t\t\t\tif (row[\"{name}\"] != DBNull.Value)\r\n");
                if (type == "ToString")
                    result.Append($"\t\t\t\t\tmodel.{name} = row[\"{name}\"].{type}();\r\n");
                else
                    result.Append($"\t\t\t\t\tmodel.{name} = Convert.{type}(row[\"{name}\"]);\r\n");
            }
            result.Append($"\t\t\t\tlist.Add(model);\r\n");
            result.Append($"\t\t\t}}\r\n");
            result.Append($"\t\t\treturn list;\r\n");
            result.Append($"\t\t}}\r\n");

            return result.ToString();
        }

        private string SetGetPager()
        {
            StringBuilder result = new StringBuilder(300);

            result.Append("\t\tpublic DataTable GetPager(Common.PageInfo pageInfo)\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tDataSet ds = DbHelperSQL.GetPageList(\r\n");
            result.Append("\t\t\t\ttable, field, orderBy, where, groupBy, pageInfo.PageSize, pageInfo.PageIndex, out int totalSize, out int totalPage\r\n");
            result.Append("\t\t\t);\r\n");
            result.Append("\t\t\tpageInfo.TotalSize = totalSize;\r\n");
            result.Append("\t\t\tpageInfo.TotalPage = totalPage;\r\n\r\n");
            result.Append("\t\t\treturn ds.Tables[0];\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetToString()
        {
            StringBuilder result = new StringBuilder(300);

            result.Append("\t\tpublic override string ToString()\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\treturn JointFullQueryString();\r\n");
            result.Append("\t\t}\r\n\r\n");

            return result.ToString();
        }

        private string SetJointFullQueryString()
        {
            StringBuilder result = new StringBuilder(300);

            result.Append("\t\tprivate string JointFullQueryString()\r\n");
            result.Append("\t\t{\r\n");
            result.Append("\t\t\tstring result = $\" SELECT {field} FROM {table} WHERE {where} ORDER BY {orderBy}\";\r\n");
            result.Append("\t\t\tif (!string.IsNullOrWhiteSpace(groupBy))\r\n");
            result.Append("\t\t\t\tresult += $\" GROUP BY {groupBy} \";\r\n\r\n");
            result.Append("\t\t\treturn result;\r\n");
            result.Append("\t\t}\r\n\r\n");


            return result.ToString();
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
            string path = FILE_PATH + "Table/";
            string filePath = path + name + ".cs";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(filePath, contents);
            File.Copy(ConfigurationManager.AppSettings["IDALFilePath"], FILE_PATH + "IDAL.cs", true);
        }
    }
}
