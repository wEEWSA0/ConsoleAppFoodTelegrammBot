namespace ConsoleAppFoodTelegrammBot.Bot.StateMachine;

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }

    public override string ToString()
    {
        return $"Имя - {Name}; Возраст - {Age}; Город - {City};";
    }
}