using System;
using TelegramBot;
using Telegram;
using Telegram.Bot;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "1772854762:AAEobF63_hHB7vZoW6_FdA8b4QHxULN4cPE";
            TelegramBotClient bot = new TelegramBotClient(token);
            Console.WriteLine($"@{bot.GetMeAsync().Result.Username} start...");

            bot.OnMessage += (s, e) =>
              {

                  string msgText = e.Message.Text;
                  string firstName = e.Message.Chat.FirstName;
                  string replyMsg = String.Empty;
                  int msgId = e.Message.MessageId;
                  long chatId = e.Message.Chat.Id;

                  Console.WriteLine($"{firstName}:{msgText}");


                  bot.SendTextMessageAsync( chatId: chatId,  text: replyMsg + "Получено!",  replyToMessageId: msgId); 
                  //int user = 0;
                  //string path = $"id_{chatId.ToString().Substring(0, 5).Substring(0, 5)}";
                  //bool skip = false;

              };

            bot.StartReceiving();
            Console.ReadLine();
        }


    }
}
