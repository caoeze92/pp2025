using System.Configuration;
using System.Data.SqlClient;

public static class ConnectionFactory
{
    public static SqlConnection GetConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["dbLocalMaxi"].ConnectionString;
        return new SqlConnection(connectionString);
    }
}