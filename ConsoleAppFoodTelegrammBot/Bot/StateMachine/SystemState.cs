namespace ConsoleAppFoodTelegrammBot.Bot.StateMachine;

public class SystemState
{
    public User User { get; private set; }
    public State State { get; private set; }

    public SystemState(User user, State state)
    {
        User = user;
        State = state;
    }
}