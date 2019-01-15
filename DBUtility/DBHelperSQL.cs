/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       DBHelper.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 13:04:45
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBUtility
{
    public static class DBHelperSQL
    {
        private static readonly string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private static readonly object lockObj = new object();

        private static DataTable all_Tables;

        private static DataSet all_Tables_Struct;

        /// <summary>
        /// 查询数据库所有的表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllTables()
        {
            if (all_Tables == null)
            {
                lock (lockObj)
                {
                    if (all_Tables == null)
                    {
                        all_Tables = Query("select name from sysobjects where xtype='U';").Tables[0];
                    }
                }
            }
            return all_Tables;
        }

        /// <summary>
        /// 获取所有表结构
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllTablesStruct()
        {
            if (all_Tables == null)
                GetAllTables();

            if (all_Tables_Struct == null)
            {
                lock (lockObj)
                {
                    if (all_Tables_Struct == null)
                    {
                        all_Tables_Struct = _GetAllTablesStruct();
                    }
                }
            }
            return all_Tables_Struct;
        }

        private static DataSet _GetAllTablesStruct()
        {
            StringBuilder querySQL = new StringBuilder("SP_COLUMNS ", 500);

            querySQL.Append(
                string.Join("\r\nGO\r\nSP_COLUMNS ", all_Tables.AsEnumerable().Select(row => row["name"]))
            );

            return Query(querySQL.ToString());
        }

        private static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
    }
}
