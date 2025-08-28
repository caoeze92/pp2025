using System.Configuration;
using System.Data.SqlClient;

public static class ConnectionFactory
{
    public static SqlConnection GetConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ezeDDBB"].ConnectionString;
        return new SqlConnection(connectionString);
    }
}