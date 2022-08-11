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

    public Task<Message> ProcessMessageText(ITelegramBotClient botClient, string requestMessageText, ChatId chatId,
        CancellationToken cancellationToken)
    {
        switch (requestMessageText)
        {
            case "/typesDishes": return SendTypesDishes(botClient, chatId, cancellationToken);
            case "/help": return GetHelp(botClient, chatId, cancellationToken);
            case "/hide": return HideKeyBoard(botClient, chatId, cancellationToken);
            default: return GetDefault(botClient, chatId, cancellationToken);
        }
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
}