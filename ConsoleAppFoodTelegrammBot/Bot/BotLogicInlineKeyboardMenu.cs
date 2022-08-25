using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotLogicInlineKeyboardMenu
{
    private enum CallBackData
    {
        GotoA,
        GotoB,
        GotoAA,
        GotoBB,
        GotoMainMenu
    }

    private Dictionary<string, Func<ITelegramBotClient, ChatId, int, CancellationToken, Task>> _methodsDictionary;

    public BotLogicInlineKeyboardMenu()
    {
        _methodsDictionary = new Dictionary<string, Func<ITelegramBotClient, ChatId, int, CancellationToken, Task>>();

        _methodsDictionary[CallBackData.GotoA.ToString()] = EditMenuA;
        _methodsDictionary[CallBackData.GotoB.ToString()] = EditMenuB;
        _methodsDictionary[CallBackData.GotoMainMenu.ToString()] = EditMainMenu;
    }

    public Task CallMethodByName(string callBackData, ITelegramBotClient botClient, ChatId chatId, int messageId,
        CancellationToken cancellationToken)
    {
        var method = _methodsDictionary[callBackData];
        return method.Invoke(botClient, chatId, messageId, cancellationToken);
    }

    public Task FirstSendMainMenu(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Добро пожаловать в главное меню",
            replyMarkup: GetMainMenuInlineKeyboard(),
            cancellationToken: cancellationToken);
    }

    private Task EditMenuA(ITelegramBotClient botClient, ChatId chatId, int messageId,
        CancellationToken cancellationToken)
    {
        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: "Добро пожаловать в меню А",
            replyMarkup: GetMenuAInlineKeyboard(),
            cancellationToken: cancellationToken
        );
    }

    private Task EditMenuB(ITelegramBotClient botClient, ChatId chatId, int messageId,
        CancellationToken cancellationToken)
    {
        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: "Добро пожаловать в меню B",
            replyMarkup: GetMenuBInlineKeyboard(),
            cancellationToken: cancellationToken
        );
    }

    private Task EditMainMenu(ITelegramBotClient botClient, ChatId chatId, int messageId,
        CancellationToken cancellationToken)
    {
        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: "Добро пожаловать в главное меню",
            replyMarkup: GetMainMenuInlineKeyboard(),
            cancellationToken: cancellationToken
        );
    }
    
    private InlineKeyboardMarkup GetMainMenuInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto A", CallBackData.GotoA.ToString()),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto B", CallBackData.GotoB.ToString()),
            },
        });
    }

    private InlineKeyboardMarkup GetMenuAInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto AA", CallBackData.GotoAA.ToString()),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto MainMenu", CallBackData.GotoMainMenu.ToString()),
            },
        });
    }

    private InlineKeyboardMarkup GetMenuBInlineKeyboard()
    {
        return new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto BB", CallBackData.GotoBB.ToString()),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Goto MainMenu", CallBackData.GotoMainMenu.ToString()),
            },
        });
    }
}