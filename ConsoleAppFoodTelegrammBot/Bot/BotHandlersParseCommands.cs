using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlersParseCommands
{
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;

            string responseMessageText = "unknown";

            //    /calc 2+5
            if (requestMessageText.StartsWith("/calc"))
            {
                string expression = requestMessageText.Substring(requestMessageText.IndexOf(' ') + 1);
                expression = expression.Trim();

                Regex regex = new Regex("([0-9]+)([\\+,\\-,\\*,\\/])([0-9]+)");
                Match match = regex.Match(expression);
                
                double a = double.Parse(match.Groups[1].Value);
                string sign  = match.Groups[2].Value;
                double b = double.Parse(match.Groups[3].Value);

                switch (sign)
                {
                    case "+":
                        responseMessageText = (a + b).ToString();
                        break;
                    case "-":
                        responseMessageText = (a - b).ToString();
                        break;
                    case "*":
                        responseMessageText = (a * b).ToString();
                        break;
                    case "/":
                        responseMessageText = (a / b).ToString("F2");
                        break;
                }
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
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