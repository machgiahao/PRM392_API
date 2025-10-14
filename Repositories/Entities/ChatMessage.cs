namespace Repositories.Entities;

public class ChatMessage
{
    public int ChatMessageId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime SentAt { get; set; }

    public User? User { get; set; }
}
