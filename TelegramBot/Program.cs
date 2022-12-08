using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Program
    {
        private static readonly string language = "ru";
        private static readonly string units = "metric";
        
        private static TelegramBotClient client;
        
        private static Parameters parameters;

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            parameters = ParametersSerializer.Deserialize();

            client = new TelegramBotClient(parameters.TelegramApiKey);
            client.StartReceiving(Update, Error, null);
            Console.ReadLine();

            parameters.Serialize();
        }

        private async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (message != null)
            {
                if (message.Text != null)
                {
                    Console.WriteLine($"@{message.Chat.Username ?? "Anonimus"}:  {message.Text}");
                    switch (message.Text.ToLower())
                    {
                        case "/start":
                            await client.SendTextMessageAsync(message.Chat.Id, "Давайте начнём!");
                            break;
                    }
                }
                if(message.Location != null)
                {
                    Console.WriteLine($"@{message.Chat.Username ?? "Anonimus"}:  {message.Location.Latitude} {message.Location.Longitude}");
                    using (var httpclient = new HttpClient())
                    {
                        double lattitude = Math.Round(message.Location.Latitude, 2);
                        double longitude = Math.Round(message.Location.Longitude, 2);

                        var response = await httpclient.GetAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lattitude}&lon={longitude}&lang={language}&units={units}&appid={parameters.WheatherApiKey}")); ;
                        string result = await response.Content.ReadAsStringAsync();
                        
                        WeatherJson weather = JsonSerializer.Deserialize<WeatherJson>(result);
                        
                        await client.SendTextMessageAsync(message.Chat.Id, weather.ToString());
                    }
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
