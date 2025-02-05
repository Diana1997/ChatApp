using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<UserSession> UserSessions => _database.GetCollection<UserSession>("UserSessions");
        public IMongoCollection<Room> Rooms =>  _database.GetCollection<Room>("Rooms");
        public IMongoCollection<ChatMessage> ChatMessages => _database.GetCollection<ChatMessage>("ChatMessages");
    }
}