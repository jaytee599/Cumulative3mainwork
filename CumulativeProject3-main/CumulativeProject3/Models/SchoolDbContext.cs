using MySql.Data.MySqlClient;

namespace CumulativeProject3.Models;

public class SchoolDbContext
{
    private const string User = "root";
    private const string Password = "";
    private const string Database = "schooldb";
    private const string Server = "localhost";
    private const string Port = "3306";

    private static string ConnectionString => $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Password};ConvertZeroDateTime=True";

    /// <summary>
    /// Returns a connection to the school database.
    /// </summary>
    /// <example>
    /// private SchoolDbContext School = new SchoolDbContext();
    /// MySqlConnection Conn = School.Connection();
    /// </example>
    /// <returns>A MySqlConnection Object</returns>
    public MySqlConnection Connection()
    {
        return new MySqlConnection(ConnectionString);
    }
}