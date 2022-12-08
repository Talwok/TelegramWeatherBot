public class Parameters
{
    public string WheatherApiKey { get; set; } //openweathermap.org
    public string TelegramApiKey { get; set; } //TelegramBot

    public Parameters()
    {
        WheatherApiKey = "none";
        TelegramApiKey = "none";
    }
}