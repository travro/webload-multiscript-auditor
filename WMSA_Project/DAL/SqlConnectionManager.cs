using System;
using System.Data;
using System.Data.SqlClient;

namespace WMSA_Project.DAL
{
    public static class SqlConnectionManager
    {
        private static SqlConnection GetOpenMasterConnection()
        {
            var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["master"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        public static SqlConnection GetOpenConnection()
        {
            var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        public static bool CheckDatabaseAvailability()
        {
            try
            {
                var cnn = GetOpenConnection();
                if (cnn != null)
                {
                    cnn.Close();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}

