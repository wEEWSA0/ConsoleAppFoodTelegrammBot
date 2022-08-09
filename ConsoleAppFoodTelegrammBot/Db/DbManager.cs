using ConsoleAppFoodTelegrammBot.Db.Table;

namespace ConsoleAppFoodTelegrammBot.Db;

public class DbManager
{
    private static DbManager _dbManager = null;

    public TableTypesDishes TableTypesDishes { private set; get; }

    private DbManager()
    {
        TableTypesDishes = new TableTypesDishes();
    }

    public static DbManager GetInstance()
    {
        if (_dbManager == null)
        {
            _dbManager = new DbManager();
        }

        return _dbManager;
    }
}