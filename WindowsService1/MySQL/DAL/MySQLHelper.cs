using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace WindowsService1.MySQL.DAL
{
     class MySQLHelper
    {
        private static readonly string connectionString = "server=localhost;port=3306;user=root;password=libaoping; database=jucheapcore;";
        MySqlConnection conn = new MySqlConnection(connectionString);
        //返回受影响的行数
        public static int ExecuteNonQuery(string sqlStr, params MySqlParameter[] paras)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {

                    cmd.CommandText = sqlStr;
                    //是否需要对参数有无进行判断
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        //返回第一行的第一列
        public static object ExecuteScalar(string sqlStr, params MySqlParameter[] paras)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddRange(paras);
                    return cmd.ExecuteScalar();
                }
            }

        }
        public static DataTable ExecuteDataTable(string sqlStr, params MySqlParameter[] paras)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddRange(paras);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    //MySqlDataReader dr = cmd.ExecuteReader();
                    //dt.Load(dr);
                    //dr.Close();
                    da.Fill(dt);
                    return dt;
                }
            }






        }

    }
}
