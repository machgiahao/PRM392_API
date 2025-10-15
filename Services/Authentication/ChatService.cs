using Repositories.Uow;
using Repositories.Entities;
using Services.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync()
        {
            var chatRepository = _unitOfWork.Repository<ChatMessage>();
            var messages = await chatRepository.GetQueryable()
                .Include(c => c.User)
                .OrderBy(c => c.SentAt)
                .Select(c => new ChatMessageDto
                {
                    ChatMessageId = c.ChatMessageId,
                    UserId = c.UserId,
                    Username = c.User != null ? c.User.Username : "Unknown",
                    Message = c.Message,
                    SentAt = c.SentAt
                })
                .ToListAsync();

            return messages;
        }
    }
}