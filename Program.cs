using System;
using TelegramBot;
using Telegram;
using Telegram.Bot;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using Telegram.Bot.Types.InputFiles;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "1772854762:AAEobF63_hHB7vZoW6_FdA8b4QHxULN4cPE";
            TelegramBotClient bot = new TelegramBotClient(token);
            Console.WriteLine($"@{bot.GetMeAsync().Result.Username} start... ");

            int max = 5;
            Random rand = new Random();
            Dictionary<long, int> db = new Dictionary<long, int>();



            bot.OnMessage += (s, e) =>
              {

                  string msgText = e.Message.Text;
                  string firstName = e.Message.Chat.FirstName;
                  string replyMsg = String.Empty;
                  int msgId = e.Message.MessageId;
                  long chatId = e.Message.Chat.Id;

                  string path = $"id{chatId.ToString().Substring(0, 5)}";

                  Console.WriteLine($"{firstName}:{msgText}");
                  int user;
                  bool skip = false;



                  if (!db.ContainsKey(chatId)
                      || msgText.ToLower() == "/restart"
                      || msgText.ToLower().IndexOf("start") != -1)
                  {
                      int startGame = rand.Next(20, 30);
                      db[chatId] = startGame;
                      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                      skip = true;
                      replyMsg = $"Загаданное число : {db[chatId]}";
                  }

                  else
                  {
                      if (db[chatId] <= 0) return;

                      int.TryParse(msgText, out user);

                      if (!(user >= 1 && user <= max))

                      {
                          skip = true;
                          replyMsg = $"Обнаружено читерство. Число {db[chatId]}";
                      }
                      if (!skip)
                      {
                          db[chatId] -= user; // Вычитание числа
                          replyMsg = $"Ход {firstName}: {user}. Число {db[chatId]}";
                          if (db[chatId] <= 0)
                          {
                              replyMsg = $"Ура! Победа, {firstName} !";
                              skip = true;
                          }
                      }
                  }

                  if (!skip)
                  {
                      int temp = rand.Next(max) + 1;
                      db[chatId] -= temp;
                      replyMsg += $"\nХод Бота: {temp} Число: {db[chatId]}";
                      if (db[chatId] <= 0) replyMsg = replyMsg = $"Ура! Победа Бота !";
                  }

                  Bitmap image = new Bitmap(600, 100);
                  Graphics graphics = Graphics.FromImage(image);
                  graphics.DrawString(
                      s: replyMsg, //+ $"  привет, {firstName}! XD)))",
                      font: new Font("Consolas", 16),
                      brush: Brushes.SeaGreen,
                      x: 10,
                      y: 40
                      ); ;


                  path += $@"\file_{DateTime.Now.Ticks}.bmp";
                  image.Save(path);

                  bot.SendPhotoAsync(
                      chatId: chatId,
                      photo: new InputOnlineFile(new FileStream(path,FileMode.Open)),
                      //caption: "htttps://t.me/joinchat/",
                      replyToMessageId: msgId

                      );



                  Console.WriteLine($" >>>> {replyMsg}");

                  bot.SendTextMessageAsync( 
                      chatId: chatId,  
                      text: replyMsg + " Получено!",  
                      replyToMessageId: msgId); 
                  //int user = 0;
                  //string path = $"id_{chatId.ToString().Substring(0, 5).Substring(0, 5)}";
                  //bool skip = false;

              };

            bot.StartReceiving();
            Console.ReadLine();
        }


    }
}
