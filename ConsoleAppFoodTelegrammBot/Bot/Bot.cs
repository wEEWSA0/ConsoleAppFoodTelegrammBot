using ConsoleAppFoodTelegrammBot.Db;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class Bot
{
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        _botClient = new TelegramBotClient("5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        //BotHandlersInlineKeyboardMenu botHandlersInlineKeyboardMenu = new BotHandlersInlineKeyboardMenu(new BotLogicInlineKeyboardMenu());

        BotHandlersSendPhotoFromComputer handlersSendPhotoFromComputer = new BotHandlersSendPhotoFromComputer();
        
        _botClient.StartReceiving(
            handlersSendPhotoFromComputer.HandleUpdateAsync,
            handlersSendPhotoFromComputer.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );
    }

    public string GetBotName()
    {
        return _botClient.GetMeAsync().Result.Username;
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        DbManager.GetInstance().CloseConnection();
    }
}