﻿using Telegram.Bot;
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
            photo: "https://raw.githubusercontent.com/jberries1/Telegram_Spotify_bot/main/BotSpotify/1%20%D0%BC%D0%B5%D1%81%D1%8F%D1%86/1.png",
            caption: "<b>➖➖➖➖➖➖➖➖➖➖➖➖\r\n📃 Товар: 1 мес. Spotify Premium \r\n💰 Цена: 280 ₽  \r\n📦 Кол-во: 1 шт.\r\n💡  Итоговая сумма: 280 ₽\r\n💲 Способ оплаты:Qiwi \r\n➖➖➖➖➖➖➖➖➖➖➖➖\r\nДля оплаты произведите перевод на Qiwi кошелек  на сумму 280р на номер +79998441525\r\n⏰ После оплаты напишите оператору: @Nette_RA</b>",
            parseMode: ParseMode.Html
        );
        return;
    }

    if (callbackQuery.Data.StartsWith("3 month"))
    {
        await botClient.SendPhotoAsync(
            callbackQuery.Message.Chat.Id,
            photo: "https://raw.githubusercontent.com/jberries1/Telegram_Spotify_bot/main/BotSpotify/1%20%D0%BC%D0%B5%D1%81%D1%8F%D1%86/2.png",
            caption: "<b>➖➖➖➖➖➖➖➖➖➖➖➖\r\n📃 Товар: 3 мес. Spotify Premium \r\n💰 Цена: 800 ₽  \r\n📦 Кол-во: 1 шт.\r\n💡  Итоговая сумма: 800 ₽\r\n💲 Способ оплаты:Qiwi \r\n➖➖➖➖➖➖➖➖➖➖➖➖\r\nДля оплаты произведите перевод на Qiwi кошелек  на сумму 800р на номер +79998441525\r\n⏰ После оплаты напишите оператору: @Nette_RA</b>",
            parseMode: ParseMode.Html
        );
        return;
    }

    if (callbackQuery.Data.StartsWith("6 month"))
    {
        await botClient.SendPhotoAsync(
            callbackQuery.Message.Chat.Id,
            photo: "https://raw.githubusercontent.com/jberries1/Telegram_Spotify_bot/main/BotSpotify/1%20%D0%BC%D0%B5%D1%81%D1%8F%D1%86/3.png",
            caption: "<b>➖➖➖➖➖➖➖➖➖➖➖➖\r\n📃 Товар: 6 мес. Spotify Premium \r\n💰 Цена: 1550 ₽  \r\n📦 Кол-во: 1 шт.\r\n💡  Итоговая сумма: 1550 ₽\r\n💲 Способ оплаты:Qiwi \r\n➖➖➖➖➖➖➖➖➖➖➖➖\r\nДля оплаты произведите перевод на Qiwi кошелек  на сумму 1550р на номер +79998441525\r\n⏰ После оплаты напишите оператору: @Nette_RA</b>",
            parseMode: ParseMode.Html
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("12 month"))
    {
        await botClient.SendPhotoAsync(
            callbackQuery.Message.Chat.Id,
            photo: "https://raw.githubusercontent.com/jberries1/Telegram_Spotify_bot/main/BotSpotify/1%20%D0%BC%D0%B5%D1%81%D1%8F%D1%86/4.png",
            caption: "<b>➖➖➖➖➖➖➖➖➖➖➖➖\r\n📃 Товар: 12 мес. Spotify Premium \r\n💰 Цена: 2560 ₽  \r\n📦 Кол-во: 1 шт.\r\n💡  Итоговая сумма: 2560 ₽\r\n💲 Способ оплаты:Qiwi \r\n➖➖➖➖➖➖➖➖➖➖➖➖\r\nДля оплаты произведите перевод на Qiwi кошелек  на сумму 2560р на номер +79998441525\r\n⏰ После оплаты напишите оператору: @Nette_RA</b>",
            parseMode: ParseMode.Html
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