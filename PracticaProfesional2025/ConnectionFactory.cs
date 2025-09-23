using System.Configuration;
using System.Data.SqlClient;

public static class ConnectionFactory
{
    public static SqlConnection GetConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        return new SqlConnection(connectionString);
    }
}