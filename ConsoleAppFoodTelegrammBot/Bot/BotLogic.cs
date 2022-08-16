using ConsoleAppFoodTelegrammBot.Db;
using ConsoleAppFoodTelegrammBot.Db.Model;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleAppFoodTelegrammBot.Bot;

public class BotLogic
{
    private DbManager _dbManager;

    public BotLogic()
    {
        _dbManager = DbManager.GetInstance();
    }

    public Task<Message> ProcessTextMessage(ITelegramBotClient botClient, string requestMessageText, ChatId chatId,
        CancellationToken cancellationToken)
    {
        switch (requestMessageText)
        {
            case "/typesDishes": return SendTypesDishes(botClient, chatId, cancellationToken);
            case "/help": return GetHelp(botClient, chatId, cancellationToken);
            case "/hide": return HideKeyBoard(botClient, chatId, cancellationToken);
            case "/inline": return SendInlineKeyBoard(botClient, chatId, cancellationToken);
            default: return GetDefault(botClient, chatId, cancellationToken);
        }
    }

    public Task<Message> ProcessCallBackMessage(ITelegramBotClient botClient, ChatId chatId, int messageId,
        string data,
        CancellationToken cancellationToken)
    {
        string text = "callback";

        switch (data)
        {
            case "11":
                text += "1111111";
                break;
            case "22":
                text += "222222";
                break;
        }

        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: "sdfsdfsdf",
            cancellationToken: cancellationToken
        );


        // return botClient.SendTextMessageAsync(
        //     chatId: chatId,
        //     text: text,
        //     cancellationToken: cancellationToken);
    }


    private Task<Message> SendTypesDishes(ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        List<TypeDish> typesDishes = _dbManager.TableTypesDishes.GetAllTypesDishes();
        KeyboardButton[][] keyboardButtons = new KeyboardButton[typesDishes.Count + 1][];

        for (int i = 0; i < typesDishes.Count; i++)
        {
            keyboardButtons[i] = new KeyboardButton[] { typesDishes[i].Name };
        }

        keyboardButtons[keyboardButtons.GetLength(0) - 1] = new KeyboardButton[] { "/hide" };

        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            keyboardButtons
        );

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите тип блюда",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    private Task<Message> HideKeyBoard(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Removing keyboard",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    private Task<Message> GetHelp(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        string responseText = "Бот, позволяющий делать заказ в ресторане\n" +
                              "/help - помощь\n" +
                              "/typesDishes - выдать список десертов";

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseText,
            cancellationToken: cancellationToken);
    }

    private Task<Message> GetDefault(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
    {
        string responseText = "Ошибка. Команда не распознана. Для вывода списка всех команд напишите /help";

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseText,
            cancellationToken: cancellationToken);
    }

    private Task<Message> SendInlineKeyBoard(ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "1.1", callbackData: "11"),
                InlineKeyboardButton.WithCallbackData(text: "1.2", callbackData: "12"),
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "2.1", callbackData: "21"),
                InlineKeyboardButton.WithCallbackData(text: "2.2", callbackData: "22"),
            },
        });

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "A message with an inline keyboard markup",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}