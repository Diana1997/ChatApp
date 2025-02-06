using Application._Common.Interfaces;
using Domain;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class AppDbContext  : IAppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDBSettings:ConnectionString"] );
            _database = client.GetDatabase(configuration["MongoDBSettings:DatabaseName"]);
            
            CreateUserNicknameIndex();
            CreateRoomNumberIndex();
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<UserSession> UserSessions => _database.GetCollection<UserSession>("UserSessions");
        public IMongoCollection<Room> Rooms =>  _database.GetCollection<Room>("Rooms");
        public IMongoCollection<ChatMessage> ChatMessages => _database.GetCollection<ChatMessage>("ChatMessages");
        public IMongoCollection<EventLog> EventLogs => _database.GetCollection<EventLog>("EventLogs");
        public IMongoCollection<WebSocketConnection> WebSocketConnections => _database.GetCollection<WebSocketConnection>("WebSocketConnections");

        private void CreateUserNicknameIndex()
        {
            var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Nickname);
            var indexOptions = new CreateIndexOptions { Unique = true };
            Users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeys, indexOptions));
        }
        
        private void CreateRoomNumberIndex()
        {
            var indexKeys = Builders<Room>.IndexKeys.Ascending(r => r.Number);
            var indexOptions = new CreateIndexOptions { Unique = true };
            Rooms.Indexes.CreateOne(new CreateIndexModel<Room>(indexKeys, indexOptions));
        }
    }
}