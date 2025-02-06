using System;
using MongoDB.Bson;

namespace Domain
{
    public class ChatMessage : BaseEntity
    {
        public string RoomId { get; set; } 
        public ObjectId UserId { get; set; }
        public string Message { get; set; } 
    }
}