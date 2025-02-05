using System;

namespace Application.ChatMessages.Queries.GetAll
{
    public class ChatMessageDto
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }
    }
}