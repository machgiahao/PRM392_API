using Microsoft.AspNetCore.Mvc;
using Services.Chat;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory()
        {
            var history = await _chatService.GetChatHistoryAsync();
            return Ok(history);
        }
    }
}