using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleAppFoodTelegrammBot.Bot.StateMachine;

public class SystemStatesManager
{
    private Dictionary<ChatId, SystemState> _systemStatesDictionary;
    private BotLogic _botLogic;

    public SystemStatesManager()
    {
        _systemStatesDictionary = new Dictionary<ChatId, SystemState>();
        _botLogic = new BotLogic();
    }

    public Task ProcessMessage(Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ChatId chatId = null;

        switch (update.Type)
        {
            case UpdateType.Message:
                chatId = update.Message.Chat.Id;
                break;

            case UpdateType.CallbackQuery:
                chatId = update.CallbackQuery.Message.Chat.Id;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (!_systemStatesDictionary.ContainsKey(chatId))
        {
            SystemState newSystemState = new SystemState(new User(), new State(State.StateVariant.WaitingStart));
            _systemStatesDictionary[chatId] = newSystemState;
        }

        SystemState currentSystemState = _systemStatesDictionary[chatId];
        return _botLogic.CallMethodByState(update, currentSystemState, botClient, chatId, cancellationToken);
    }
}