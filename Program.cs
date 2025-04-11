using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    static TelegramBotClient bot;

    static void Main(string[] args)
    {
        bot = new TelegramBotClient("7822764030:AAFoyIwadcsRHQzIOXnsdGsxbBAVoMIv0qw");

        bot.OnMessage += Bot_OnMessage;
        bot.StartReceiving();

        Console.WriteLine("🤖 Support Bot started. Press any key to exit.");
        Console.ReadLine(); // Or Thread.Sleep(-1) if you prefer
    }

    static async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.Message.Type != MessageType.Text) return;

        var chatId = e.Message.Chat.Id;
        var message = e.Message.Text?.ToLower();

        if (message == "/start")
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "📄 FAQs", "💬 Talk to support" },
                new KeyboardButton[] { "ℹ️ Service info" }
            })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: "👋 Welcome to SupportBot! How can I help you today?",
                replyMarkup: replyKeyboard
            );
        }
        else if (message.Contains("faq"))
        {
            await bot.SendTextMessageAsync(chatId, "📄 FAQ:\n1. How to use the service?\n2. How to contact support?\n...");
        }
        else if (message.Contains("support"))
        {
            await bot.SendTextMessageAsync(chatId, "💬 A human agent will be with you shortly. Please describe your issue.");
        }
        else if (message.Contains("service"))
        {
            await bot.SendTextMessageAsync(chatId, "ℹ️ We offer 24/7 customer support and service monitoring.");
        }
        else
        {
            await bot.SendTextMessageAsync(chatId, $"📩 You said: {e.Message.Text}");
        }
    }
}
