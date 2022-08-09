using ConsoleAppFoodTelegrammBot.Db.Connection;
using ConsoleAppFoodTelegrammBot.Db.Model;
using Npgsql;

namespace ConsoleAppFoodTelegrammBot.Db.Table;

public class TableTypesDishes
{
    private NpgsqlConnection _connection;

    public TableTypesDishes()
    {
        _connection = DbConnector.GetConnection();
    }

    public List<TypeDish> GetAllTypesDishes()
    {
        List<TypeDish> typesDishes = new List<TypeDish>();

        string sqlRequest = "SELECT * FROM types_dishes";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string name = dataReader.GetString(dataReader.GetOrdinal("name"));

            TypeDish typeDish = new TypeDish()
            {
                Id = id,
                Name = name
            };

            typesDishes.Add(typeDish);
        }

        return typesDishes;
    }
}