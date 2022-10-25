using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5368261481:AAGBLIvI5dGbg8DM82eCjNwnnlCHKo-3voA");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};

botClient.StartReceiving(
    HandleUpdatesAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начал прослушку @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }

    if (update.Type == UpdateType.CallbackQuery)
    {
        await HandleCallbackQuery(botClient, update.CallbackQuery);
        return;
    }
}

async Task HandleMessage(ITelegramBotClient botClient, Message message)
{
    if (message.Text == "/start")
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Привет, 🖐" +
            "\r\n\r\nSpotify ушел с российского рынка и ты не можешь слушать музыку на своём любимом сервисе? Не беда, мы тебе поможем!" +
            "\r\n\r\nКак это работает? Мы оплачиваем Spotify Premium через Ваш аккаунт на выбранный период. После подключения Вы сможете использовать приложение без ограничений и VPN. Срок завершения подписки удобно можно отследить в настройках профиля. Всё просто! 🎧" +
            "\r\n\r\nПосле оплаты Вы отправляете данные от аккаунта, всю работу по подключению мы берем на себя😁\r\n\r\n📣Поддержка по любым вопросам - @Nette_RA");



        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] {"Индивидуальная подписка", "Помощь"},
            new KeyboardButton[] {"Отзывы", "Промокод"}
        })
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose:", replyMarkup: keyboard);
        return;
    }
    if (message.Text == "Индивидуальная подписка")
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "📃 Категория: Spotify Premium Индивидуальная\r\n📃 Описание: После оплаты на Вашем личном аккаунте будет оплачен Premium на выбранный срок.");

        InlineKeyboardMarkup keyboard1 = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1 месяц - Spotify Premium / 280 Р", "1 month"),


            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("3 месяца - Spotify Premium / 800 Р", "3 month")

            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("6 месяцев - Spotify Premium / 1550 Р", "6 month")

            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("12 месяцев - Spotify Premium / 2560 Р", "12 month")

            }

        });
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose inline:", replyMarkup: keyboard1);
        return;

    }

    if (message.Text == "/inline")
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Buy 50c", "buy_50c"),
                InlineKeyboardButton.WithCallbackData("Buy 100c", "buy_100c"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Sell 50c", "sell_50c"),
                InlineKeyboardButton.WithCallbackData("Sell 100c", "sell_100c"),
            },
        });
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose inline:", replyMarkup: keyboard);
        return;
    }

    await botClient.SendTextMessageAsync(message.Chat.Id, $"You said:\n{message.Text}");
}
async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    if (callbackQuery.Data.StartsWith("1 month"))
    {
        await botClient.SendPhotoAsync(
            callbackQuery.Message.Chat.Id,
            photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
            caption: "<b>Произведите оплату по прикрепленной ссылке:</b> <a href=\"https://oplata.qiwi.com/form?invoiceUid=23ec566d-32e1-42e9-aea9-d45ff07d02b3\r\n\">Pay Spotify - 1m</a>",
            parseMode: ParseMode.Html
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("sell"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Вы хотите продать?"
        );
        return;
    }
    await botClient.SendTextMessageAsync(
        callbackQuery.Message.Chat.Id,
        $"You choose with data: {callbackQuery.Data}"
        );
    return;
}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}