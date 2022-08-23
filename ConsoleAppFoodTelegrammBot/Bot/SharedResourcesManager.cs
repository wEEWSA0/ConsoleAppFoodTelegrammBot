using Telegram.Bot.Types;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class SharedResourcesManager
{
    private Dictionary<ChatId, SharedResource> _sharedResourcesDictionary;
    private Random _random;

    public SharedResourcesManager()
    {
        _sharedResourcesDictionary = new Dictionary<ChatId, SharedResource>();
        _random = new Random();
    }

    public void RandomValue(ChatId chatId)
    {
        if (_sharedResourcesDictionary.ContainsKey(chatId))
        {
            SharedResource sharedResource = _sharedResourcesDictionary[chatId];
            sharedResource.X = _random.Next(1000);
        }
        else
        {
            SharedResource sharedResource = new SharedResource();
            sharedResource.X = _random.Next(1000);

            _sharedResourcesDictionary[chatId] = sharedResource;
        }
    }

    public int GetValue(ChatId chatId)
    {
        if (_sharedResourcesDictionary.ContainsKey(chatId))
        {
            return _sharedResourcesDictionary[chatId].X;
        }
        else
        {
            return -1;
        }
    }
}