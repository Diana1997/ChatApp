using System;
using MongoDB.Bson;

namespace Domain
{
    public class UserSession : BaseEntity
    {
        public string SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
        public ObjectId UserId { set; get; }
        public User User { set; get; }
    }
}