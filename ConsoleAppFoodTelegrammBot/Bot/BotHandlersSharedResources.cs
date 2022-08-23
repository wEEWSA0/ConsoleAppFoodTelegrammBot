using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlersSharedResources
{
    private static SharedResourcesManager _sharedResourcesManager = new SharedResourcesManager();
    private static Random _random = new Random();


    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;
            
            Console.WriteLine(chatId);

            string resposeMessageText = "1";

            if (requestMessageText == "/rnd")
            {
                _sharedResourcesManager.RandomValue(chatId);
                resposeMessageText = "randomized";
            }
            else if (requestMessageText == "/get")
            {
                resposeMessageText = _sharedResourcesManager.GetValue(chatId).ToString();
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: resposeMessageText,
                cancellationToken: cancellationToken);
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