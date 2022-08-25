using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlersSendPhotoFromComputer
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;

            if (requestMessageText == "/start")
            {
                InputOnlineFile file = new InputOnlineFile(new FileStream("smile.png", FileMode.Open));

                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: file,
                    cancellationToken: cancellationToken);
                // await botClient.SendTextMessageAsync(
                //     chatId: chatId,
                //     text: "Фото",
                //     cancellationToken: cancellationToken);
            }
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
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