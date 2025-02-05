using Domain;
using MongoDB.Driver;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<UserSession> UserSessions { get; }
        IMongoCollection<Room> Rooms { get; }
        IMongoCollection<ChatMessage> ChatMessages { get; }
        
    }
}