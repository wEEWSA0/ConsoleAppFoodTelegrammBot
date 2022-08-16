using Npgsql;

namespace ConsoleAppFoodTelegrammBot.Db.Connection;

public class DbConnector
{
    private static DbConnector _connector = null;

    private const string _connectionString =
        "Host=194.67.105.79;Username=foodbot208user;Password=12345;Database=foodbot208db";

    public NpgsqlConnection Connection { private set; get; }
    
    private DbConnector()
    {
        Connection = new NpgsqlConnection(_connectionString);
        Connection.Open();
    }

    public static DbConnector GetInstance()
    {
        if (_connector == null)
        {
            _connector = new DbConnector();
        }

        return _connector;
    }
}