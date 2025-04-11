using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    static ITelegramBotClient bot;

    static void Main()
    {
        // 🔐 Your bot token
        bot = new TelegramBotClient("7822764030:AAFoyIwadcsRHQzIOXnsdGsxbBAVoMIv0qw");

        // 🌐 TEMPORARY: SSL bypass for local dev
        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;

        // 📨 Message listener
        bot.OnMessage += Bot_OnMessage;

        // ▶️ Start receiving updates (polling)
        bot.StartReceiving();
        Console.WriteLine("🤖 Bot is running. Press Enter to exit.");
        Console.ReadLine();

        // ⏹ Stop gracefully
        bot.StopReceiving();
    }

    static async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        try
        {
            if (e.Message?.Text == null)
                return;

            var chatId = e.Message.Chat.Id;
            var messageText = e.Message.Text.ToLower();

            Console.WriteLine($"📩 Received from {chatId}: {messageText}");

            // Handle replies
            switch (messageText)
            {
                case "/start":
                    var welcomeText = "👋 Welcome to Customer Care!\nChoose an option below:";
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton[] { "💼 Billing", "📦 Orders" },
                        new KeyboardButton[] { "🔧 Technical Support", "❌ Hide Menu" }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    await bot.SendTextMessageAsync(chatId, welcomeText, replyMarkup: replyKeyboard).ConfigureAwait(false);
                    break;

                case "💼 billing":
                    await bot.SendTextMessageAsync(chatId, "💼 Billing support: Please describe your issue.").ConfigureAwait(false);
                    break;

                case "📦 orders":
                    await bot.SendTextMessageAsync(chatId, "📦 Orders support: Please provide your order number.").ConfigureAwait(false);
                    break;

                case "🔧 technical support":
                    await bot.SendTextMessageAsync(chatId, "🔧 Tech support: Tell us what you're experiencing.").ConfigureAwait(false);
                    break;

                case "❌ hide menu":
                    var removeKeyboard = new ReplyKeyboardRemove();
                    await bot.SendTextMessageAsync(chatId, "✅ Menu hidden. Type /start to bring it back.", replyMarkup: removeKeyboard).ConfigureAwait(false);
                    break;

                default:
                    await bot.SendTextMessageAsync(chatId, "✅ Got it! A support agent will follow up soon.").ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("⚠️ Error handling message:");
            Console.WriteLine(ex.ToString());
        }
    }
}
