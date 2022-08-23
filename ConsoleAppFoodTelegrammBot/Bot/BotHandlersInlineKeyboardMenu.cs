using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotHandlersInlineKeyboardMenu
{
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;

            if (requestMessageText == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Добро пожаловать в главное меню",
                    replyMarkup: GetMainMenuInlineKeyboard(),
                    cancellationToken: cancellationToken);
            }
        }
        else if (update.CallbackQuery != null)
        {
            ChatId chatId = update.CallbackQuery.Message.Chat.Id;
            int messageId = update.CallbackQuery.Message.MessageId;
            string callBackData = update.CallbackQuery.Data;
            
            string text = "1";
            InlineKeyboardMarkup keyboardMarkup = null;

            switch (callBackData)
            {
                case "goto_a":
                    text = "Добро пожаловать в меню А";
                    keyboardMarkup = GetMenuAInlineKeyboard();
                    break;

                case "goto_b":
                    text = "Добро пожаловать в меню B";
                    keyboardMarkup = GetMenuBInlineKeyboard();
                    break;

                case "goto_mainmenu":
                    text = "Добро пожаловать в главное меню";
                    keyboardMarkup = GetMainMenuInlineKeyboard();
                    break;
            }

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: keyboardMarkup,
                cancellationToken: cancellationToken
            );
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


    private static InlineKeyboardMarkup GetMainMenuInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto A", callbackData: "goto_a"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto B", callbackData: "goto_b"),
            },
        });
    }

    private static InlineKeyboardMarkup GetMenuAInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto AA", callbackData: "goto_a_a"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto MainMenu", callbackData: "goto_mainmenu"),
            },
        });
    }

    private static InlineKeyboardMarkup GetMenuBInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto BB", callbackData: "goto_a_b"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Goto MainMenu", callbackData: "goto_mainmenu"),
            },
        });
    }
}