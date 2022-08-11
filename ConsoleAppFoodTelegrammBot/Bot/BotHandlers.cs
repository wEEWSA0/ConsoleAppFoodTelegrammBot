using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlers
{
    private static BotLogic _botLogic = new BotLogic();

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message == null)
        {
            return;
        }

        if (update.Message.Text == null)
        {
            return;
        }

        string requestMessageText = update.Message.Text;
        ChatId chatId = update.Message.Chat.Id;

        await _botLogic.ProcessMessageText(botClient, requestMessageText, chatId, cancellationToken);
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