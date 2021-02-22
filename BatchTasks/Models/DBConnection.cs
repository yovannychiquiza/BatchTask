using BatchTask.Facade;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace BatchTask.Models
{
    public class DBConnection
    {
        public static string Connection_silfab_ca = Settings.Connection_silfab_ca;
        public static string Connection_ELimg = Settings.Connection_ELimg;
        public static SqlConnection getNAVConnection()
        {
           
            string connectionString = Connection_silfab_ca;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                return sqlConnection;
            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
                return sqlConnection;
            }
        }
        public static SqlConnection getELimgConnection()
        {
            string connectionString = Connection_ELimg;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                return sqlConnection;
            }
            catch (Exception e)
            {
                Util.CreateLog(e.Message);
                return sqlConnection;
            }
        }

        public static SqlDataReader getDataFromNAVPallets()
        {
            SqlConnection sqlConnection = DBConnection.getNAVConnection();
            string selectQueryString =
            " ";

            SqlCommand sqlCommand = new SqlCommand(selectQueryString, sqlConnection);
            SqlDataReader myReader = sqlCommand.ExecuteReader();
            return myReader;

        }


    }
}
