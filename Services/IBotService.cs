using Telegram.Bot;
namespace UserApi.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}