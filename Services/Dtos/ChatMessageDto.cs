using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Services.Dtos
{
    public class ChatMessageDto
    {
        public int ChatMessageId { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string? Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
