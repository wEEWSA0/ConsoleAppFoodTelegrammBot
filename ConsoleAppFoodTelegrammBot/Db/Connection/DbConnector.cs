using Npgsql;

namespace ConsoleAppFoodTelegrammBot.Db.Connection;

public class DbConnector
{
    private static DbConnector _connector = null;

    private const string _connectionString =
        "Host=194.67.105.79;Username=foodbot208user;Password=12345;Database=foodbot208db";

    private NpgsqlConnection _connection;

    private DbConnector()
    {
        _connection = new NpgsqlConnection(_connectionString);
        _connection.Open();
    }

    public static NpgsqlConnection GetConnection()
    {
        if (_connector == null)
        {
            _connector = new DbConnector();
        }

        return _connector._connection;
    }
}