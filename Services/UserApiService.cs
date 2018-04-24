using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UserApi.Controllers;
using UserApi.Models;
using Newtonsoft.Json;
namespace UserApi.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UserApiService> _logger;

        private readonly UserContext _context;

        public UserApiService(IBotService botService, ILogger<UserApiService> logger, UserContext context)
        {
            _botService = botService;
            _logger = logger;
            _context = context;
        }

        public async Task CallApi(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            var message = update.Message;

            _logger.LogInformation("Received Message from {0}", message.Chat.Id);

            if (message.Type == MessageType.Text)
            {
                // Call Api
                _logger.LogWarning("Message entity {0}", message.EntityValues);
                switch (message.Text.Split(' ').First())
                {
                    case "/help":
                        const string usage = @"
Usage:
/help - For help
/adduser - To add user (/adduser firstName lastName fathersName Date)
/infoall - Show all users
/infoid - Show user by ID (/infoid ID)
/deleteuser - To delete user (/deleteuser ID)";
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, usage);
                        break;
                    case "/adduser":
                        if (message.Text.Split(' ').Count() == 5)
                        {

                            _logger.LogWarning("Message entity {0}", _context.UserItems.Add(new UserItem
                            {
                                firstName = message.Text.Split(' ').ElementAt(1),
                                lastName = message.Text.Split(' ').ElementAt(2),
                                fathersName = message.Text.Split(' ').ElementAt(3),
                                birthDay = DateTime.Parse(message.Text.Split(' ').ElementAt(4))
                            }));
                            _context.SaveChanges();
                            await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Succsessful");
                        }
                        else
                            await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Bad request");
                        break;
                    case "/infoall":
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, JsonConvert.SerializeObject(_context.UserItems));
                        break;
                    case "/infoid":
                        {
                            if (message.Text.Split(' ').Count() == 2)
                            {
                                long id = Convert.ToInt64(message.Text.Split(' ').ElementAt(1));
                                var item = _context.UserItems.FirstOrDefault(t => t.id == id);
                                if (item == null)
                                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "NULL");
                                else
                                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, JsonConvert.SerializeObject(item));
                            }
                            else
                                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Bad request");
                        }
                        break;
                    case "/deleteuser":
                        {
                            if (message.Text.Split(' ').Count() == 2)
                            {
                                long id = Convert.ToInt64(message.Text.Split(' ').ElementAt(1));
                                var item = _context.UserItems.FirstOrDefault(t => t.id == id);
                                if (item == null)
                                {
                                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "NULL");
                                }
                                _context.UserItems.Remove(item);
                                _context.SaveChanges();
                                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Succsessful");
                            }
                            else
                                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Bad request");
                        }
                        break;
                    default:
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Please call /help for commands!");
                        break;
                }
            }
            else
            {
                // TODO: make other services 2 bots kill chat
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Please call /help for commands!");
            }
        }
    }
}
