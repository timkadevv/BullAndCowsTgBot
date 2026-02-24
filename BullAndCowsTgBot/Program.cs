using System;
using System.Diagnostics.Tracing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;



namespace TGBotTestConsoleApp
{
    internal class Program
    {
        static string token = "8679416912:AAF5VMziW4u5NIRVOBFWsULB7nG0Plf_NZQ";
        static TelegramBotClient bot = new TelegramBotClient(token);
        static List<string> words = ["корова", "машина", "школа", "бык", "луна", "земля", "дом", "рыба", "мама", "бабушка", "спорт", "город", "село", "мир", "кот", "собака", "воробей", "колибри"];

        static Random rnd = new Random();

        static string currWord;

        static StringBuilder userWord = new StringBuilder();
        static InlineKeyboardMarkup keyboard;
        static void Main(string[] args)
        {

            bot.OnMessage += Bot_OnMessage;

            Console.ReadKey();
        }

        private static async Task Bot_OnMessage(Message message, Telegram.Bot.Types.Enums.UpdateType type)
        {

            if (message.Text == "/start")
            {

                currWord = words[rnd.Next(0, words.Count)];

                userWord = new StringBuilder(new string('*', currWord.Length));

                await bot.SendMessage(message.Chat.Id, $"Новое слово из {currWord.Length} букв загадано:\n\n{userWord}\n\nНАЧИНАЕМ!");
            }
            else {

                if (userWord.Length == 0)
                {
                    await bot.SendMessage(message.Chat.Id, "Введите команду '/start', чтобы я загадал слово!");
                    return;
                }

                bool flag = true;
                int bulls = 0, cows = 0;

                if (message.Text.Length != currWord.Length)
                {
                    flag = false;

                    await bot.SendMessage(message.Chat.Id, "Вы ввели слово другой длины! Введите слово указанной длины!");
                }
                else
                {
                    for (int i = 0; i < message.Text.Length; i++)
                    {

                        char ch = message.Text[i];

                        if (!Char.IsLetter(ch))
                        {
                            flag = false;

                            await bot.SendMessage(message.Chat.Id, "Введите слово состоящее из БУКВ!");

                            break;
                        }

                        if (ch == currWord[i])
                        {

                            userWord[i] = ch;

                            bulls++;
                        }

                        else if (currWord.Contains(ch))
                        {

                            cows++;
                        }
                    }

                }
                if (flag)
                {
                    if (!userWord.ToString().Contains('*'))
                        {
                        await bot.SendMessage(message.Chat.Id, $"Ура! Вы отгадали слово:\n\n{userWord}\n\nВведите '/start' чтобы я загадал новое слово!");
                        userWord.Clear();
                    }
                    else
                    {
                        await bot.SendMessage(message.Chat.Id, $"Вы отгадали {bulls} быков и {cows} коров:\n\n{userWord}\n\nПродолжаем!");
                    }
                }
            }
        }
    }
}