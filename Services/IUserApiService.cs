using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace UserApi.Services
{
    public interface IUserApiService
    {
        Task CallApi(Update update);
    }
}
