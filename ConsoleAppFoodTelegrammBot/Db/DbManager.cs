using ConsoleAppFoodTelegrammBot.Db.Connection;
using ConsoleAppFoodTelegrammBot.Db.Table;
using Npgsql;

namespace ConsoleAppFoodTelegrammBot.Db;

public class DbManager
{
    private static DbManager _dbManager = null;

    public TableTypesDishes TableTypesDishes { private set; get; }

    private DbManager()
    {
        NpgsqlConnection connection = DbConnector.GetInstance().Connection;
        TableTypesDishes = new TableTypesDishes(connection);
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
        DbConnector.GetInstance().Connection.Close();
    }
}