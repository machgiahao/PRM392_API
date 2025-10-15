using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Chat
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync();
    }
}