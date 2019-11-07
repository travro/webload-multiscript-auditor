using System.Data.SqlClient;

namespace WMSA_Project.DAL
{
    public static class SqlConnectionManager
    {
        public static SqlConnection GetOpenConnection()
        {
            var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}

