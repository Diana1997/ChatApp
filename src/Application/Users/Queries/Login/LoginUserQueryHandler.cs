using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using MongoDB.Driver;

namespace Application.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Guid>
    {
        private readonly IAppDbContext _context;

        public LoginUserQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public  async Task<Guid> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Find(u => u.Nickname == request.Nickname)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                // Handle user not found (throw an exception or return a specific result)
                throw new UnauthorizedAccessException("User not found.");
            }

            // Generate a new session ID
            Guid sessionId = Guid.NewGuid();

            // Create a new user session
            
            var userSession = new UserSession
            {
                SessionId = sessionId,
                StartTime = DateTime.UtcNow,
                IsActive = true,
                UserId = user.Id // Assuming ObjectId is the type for user Id in MongoDB
            };

            // Insert the user session into the database
            await _context.UserSessions.InsertOneAsync(userSession, cancellationToken: cancellationToken);

            // Return the session ID
            return sessionId;
        }
    }
}