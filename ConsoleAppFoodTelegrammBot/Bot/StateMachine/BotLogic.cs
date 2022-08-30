using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleAppFoodTelegrammBot.Bot.StateMachine;

public class BotLogic
{
    private Dictionary<string, Func<Update, SystemState, ITelegramBotClient, ChatId, CancellationToken, Task>>
        _methodsDictionary;

    public BotLogic()
    {
        _methodsDictionary =
            new Dictionary<string, Func<Update, SystemState, ITelegramBotClient, ChatId, CancellationToken, Task>>();

        _methodsDictionary[State.StateVariant.WaitingStart.ToString()] = ProcessStart;
        _methodsDictionary[State.StateVariant.WaitingName.ToString()] = ProcessName;
        _methodsDictionary[State.StateVariant.WaitingAge.ToString()] = ProcessAge;
        _methodsDictionary[State.StateVariant.WaitingCity.ToString()] = ProcessCity;
    }

    public Task CallMethodByState(Update update, SystemState systemState, ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        var method = _methodsDictionary[systemState.State.GetCurrentStateName()];
        return method.Invoke(update, systemState, botClient, chatId, cancellationToken);
    }

    private Task ProcessStart(Update update, SystemState systemState, ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;

        string responseMessageText = "";

        if (requestMessageText == "/reset")
        {
            responseMessageText = "Регистрация сброшена. Для начала регистрации введите /start";
            systemState.State.SetState(State.StateVariant.WaitingStart);
        }
        else if (requestMessageText == "/start")
        {
            responseMessageText = "Отлично давайте начнём нашу регистрацию и познакомимся с Вами.\nВведите Ваше имя:";

            systemState.State.SetState(State.StateVariant.WaitingName);
        }
        else
        {
            responseMessageText = "Команда не распознана. Для начала регистрации введите /start";
        }

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    private Task ProcessName(Update update, SystemState systemState, ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;

        string responseMessageText = "";

        if (requestMessageText == "/reset")
        {
            responseMessageText = "Регистрация сброшена. Для начала регистрации введите /start";
            systemState.State.SetState(State.StateVariant.WaitingStart);
        }
        else if (requestMessageText.Length >= 2 && requestMessageText.Length <= 32)
        {
            systemState.User.Name = requestMessageText;

            responseMessageText = "Отлично имя записно.\nВведите Ваш возраст:";

            systemState.State.SetState(State.StateVariant.WaitingAge);
        }
        else
        {
            responseMessageText = "Имя не распознано. Длина имени должна быть от 2 до 32 символов. Повторите Ваш ввод";
        }

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    private Task ProcessAge(Update update, SystemState systemState, ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;

        string responseMessageText = "";

        if (requestMessageText == "/reset")
        {
            responseMessageText = "Регистрация сброшена. Для начала регистрации введите /start";
            systemState.State.SetState(State.StateVariant.WaitingStart);
        }
        else if (int.TryParse(requestMessageText, out int age) && age >= 14 && age <= 90)
        {
            systemState.User.Age = age;

            responseMessageText = "Отлично возраст записан.\nВведите Ваш город:";

            systemState.State.SetState(State.StateVariant.WaitingCity);
        }
        else
        {
            responseMessageText =
                "Возраст не распознан. Проверьте, что возраст это число от 14 до 90. Повторите Ваш ввод";
        }

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    private Task ProcessCity(Update update, SystemState systemState, ITelegramBotClient botClient, ChatId chatId,
        CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;

        string responseMessageText = "";

        if (requestMessageText == "/reset")
        {
            responseMessageText = "Регистрация сброшена. Для начала регистрации введите /start";
            systemState.State.SetState(State.StateVariant.WaitingStart);
        }
        else if (requestMessageText.Length >= 2 && requestMessageText.Length <= 32)
        {
            systemState.User.City = requestMessageText;

            responseMessageText =
                $"Отлично город записан.\nВаша регистрация закончена.\nПроверьте ваши данные\n{systemState.User}\nДля регистрации нового человека введите /start";

            systemState.State.SetState(State.StateVariant.WaitingStart);
        }
        else
        {
            responseMessageText =
                "Город не распознан. Длина города должна быть от 2 до 32 символов. Повторите Ваш ввод";
        }

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }
}