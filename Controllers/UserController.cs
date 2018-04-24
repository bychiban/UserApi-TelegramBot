using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;
using Telegram.Bot.Types;
using UserApi.Services;

namespace UserApi.Controllers {
    [Route("api/[controller]")]
    public class UserController:Controller {
        private readonly IUserApiService _userApiService;
        
        public UserController (IUserApiService userApiService){
            _userApiService = userApiService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _userApiService.CallApi(update);
            return Ok();
        }
    }
}