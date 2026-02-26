using System;
using System.Diagnostics.Tracing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Threading.Tasks;



namespace TGBotTestConsoleApp
{
    internal class Program
    {
        static string token = "8679416912:AAF5VMziW4u5NIRVOBFWsULB7nG0Plf_NZQ";
        static TelegramBotClient bot = new TelegramBotClient(token);
        static List<string> words = ["корова", "машина", "школа", "бык", "луна", "земля", "дом", "рыба", "мама", "бабушка", "спорт", "город", "село", "мир", "кот", "собака", "воробей", "колибри"];

        static Random rnd = new Random();

        static StringBuilder currWord = new StringBuilder();

        static StringBuilder userWord = new StringBuilder();
        

        static Dictionary<long, List<StringBuilder>> Bd = new Dictionary<long, List<StringBuilder>>();
        static async Task Main(string[] args)
        {

            bot.OnMessage += Bot_OnMessage;

            Console.ReadKey();
        }

        private static async Task Bot_OnMessage(Message message, Telegram.Bot.Types.Enums.UpdateType type)
        {

            if (message.Text == "/start")
            {

                currWord = new StringBuilder(words[rnd.Next(0, words.Count)]);

                userWord = new StringBuilder(new string('*', currWord.Length));

                Bd[message.Chat.Id] = [currWord, userWord];

                await bot.SendMessage(message.Chat.Id, $"Новое слово из {currWord.Length} букв загадано:\n\n{userWord}\n\nНАЧИНАЕМ!");
            }
            else {

                if (!Bd.ContainsKey(message.Chat.Id))
                {
                    await bot.SendMessage(message.Chat.Id, "Введите команду '/start', чтобы я загадал слово!");
                    return;
                }


                currWord = Bd[message.Chat.Id][0];

                userWord = Bd[message.Chat.Id][1];

                int bulls = 0, cows = 0;

                if (message.Text.Length != currWord.Length)
                {
                    
                    await bot.SendMessage(message.Chat.Id, "Вы ввели слово другой длины! Введите слово указанной длины!");
                    return;
                }

                else
                {

                    for (int i = 0; i < message.Text.Length; i++)
                    {

                        char ch = Char.ToLower(message.Text[i]);

                        if (!Char.IsLetter(ch))
                        {
                            
                            await bot.SendMessage(message.Chat.Id, "Введите слово состоящее из БУКВ!");

                            return;
                        }

                        if (ch == currWord[i])
                        {

                            userWord[i] = ch;

                            Bd[message.Chat.Id][1] = userWord;

                            bulls++;
                        }

                        else if (currWord.ToString().Contains(ch))
                        {

                            cows++;
                        }
                    }

                }

                
                if (!userWord.ToString().Contains('*'))
                    {
                    await bot.SendMessage(message.Chat.Id, $"Ура! Вы отгадали слово:\n\n{userWord}\n\nВведите '/start' чтобы я загадал новое слово!");
                    
                    userWord.Clear();
                    currWord.Clear();
                }
                else
                {
                    await bot.SendMessage(message.Chat.Id, $"Вы отгадали {bulls} быков и {cows} коров:\n\n{userWord}\n\nПродолжаем!");
                }
                
            }
        }
    }
}