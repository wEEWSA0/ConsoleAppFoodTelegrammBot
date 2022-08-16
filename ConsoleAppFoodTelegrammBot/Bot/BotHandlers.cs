using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlers
{
    private static BotLogic _botLogic = new BotLogic();
    private static Random _random = new Random();

    private static List<string> jokes = new List<string>()
    {
        "шутка1", "шутка5", "шутка4", "шутка3", "шутка2"
    };

    private static int _number = 10;

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // switch (update.Type)
        // {
        //     case UpdateType.Message:
        //         break;
        //
        //     case UpdateType.CallbackQuery:
        //         break;
        // }

        if (update.CallbackQuery != null)
        {
            await _botLogic.ProcessCallBackMessage(botClient, update.CallbackQuery.Message.Chat.Id,
                update.CallbackQuery.Message.MessageId, update.CallbackQuery.Data, cancellationToken);
        }

        if (update.Message != null && update.Message.Text != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;

            await _botLogic.ProcessTextMessage(botClient, requestMessageText, chatId, cancellationToken);
        }
    }


    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}