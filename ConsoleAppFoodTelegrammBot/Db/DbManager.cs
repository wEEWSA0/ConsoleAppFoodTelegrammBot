using ConsoleAppFoodTelegrammBot.Db.Connection;
using ConsoleAppFoodTelegrammBot.Db.Table;
using Npgsql;

namespace ConsoleAppFoodTelegrammBot.Db;

public class DbManager
{
    private static DbManager _dbManager = null;

    private NpgsqlConnection _connection;

    public TableTypesDishes TableTypesDishes { private set; get; }
    
    private DbManager()
    {
        _connection = DbConnector.GetConnection();
        TableTypesDishes = new TableTypesDishes(_connection);
    }

    public static DbManager GetInstance()
    {
        if (_dbManager == null)
        {
            _dbManager = new DbManager();
        }

        return _dbManager;
    }

    public void CloseConnection()
    {
        _connection.Close();
    }
}