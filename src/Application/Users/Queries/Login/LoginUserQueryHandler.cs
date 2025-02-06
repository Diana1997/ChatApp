using System;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Domain;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IAppDbContext _context;

        public LoginUserQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Find(u => u.Nickname == request.Nickname)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            
            string sessionId = await CreateOrUpdateUserSession(user.Id, cancellationToken);

            return sessionId;
        }

        private async Task<string> CreateOrUpdateUserSession(ObjectId userId,
            CancellationToken cancellationToken)
        {
            var existingSession = await _context.UserSessions
                .Find(session => session.UserId == userId && session.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingSession != null)
            {
                existingSession.SessionId = Guid.NewGuid().ToString();
                ;
                existingSession.StartTime = DateTime.UtcNow;
                existingSession.IsActive = true;

                await _context.UserSessions.ReplaceOneAsync(
                    session => session.Id == existingSession.Id,
                    existingSession,
                    cancellationToken: cancellationToken);


                return existingSession.SessionId;
            }

            var userSession = new UserSession
            {
                SessionId = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow,
                IsActive = true,
                UserId = userId
            };

            await _context.UserSessions.InsertOneAsync(userSession, cancellationToken: cancellationToken);

            return userSession.SessionId;
        }
    }
}