using ConsoleAppFoodTelegrammBot.Db;
using ConsoleAppFoodTelegrammBot.Db.Model;

DbManager dbManager = DbManager.GetInstance();

List<TypeDish> typesDishes = dbManager.TableTypesDishes.GetAllTypesDishes();

foreach (TypeDish typeDish in typesDishes)
{
    Console.WriteLine($"id:{typeDish.Id} name:{typeDish.Name}");
}