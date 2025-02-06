using System;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Domain;
using MongoDB.Driver;

namespace Application._Common.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IAppDbContext _context;

        public UserSessionService(IAppDbContext context)
        {
            _context = context;
        }
        
        public  bool ExistsActiveSession(string sessionId)
        {
            var exists = _context.UserSessions
                .Find(s => s.SessionId == sessionId && s.IsActive)
                .Any();

            return exists; 
        }

    

 
    }
}