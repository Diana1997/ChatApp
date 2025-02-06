using System;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Application._Common.Services;
using Domain;
using MediatR;
using MongoDB.Driver;

namespace Application.ChatMessages.Commands.ConnectToChat
{
    public class ConnectToChatCommandHandler : IRequestHandler<ConnectToChatCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly WebSocketManager _webSocketManager;

        public ConnectToChatCommandHandler(IAppDbContext context, WebSocketManager webSocketManager)
        {
            _context = context;
            _webSocketManager = webSocketManager;
        }

        public async Task<Unit> Handle(ConnectToChatCommand request, CancellationToken cancellationToken)
        {
            var userSession = await _context.UserSessions
                .Find(session => session.SessionId == request.SessionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userSession == null)
            {
                throw new UnauthorizedAccessException("Unauthorized connection.");
            }
            
            
            var roomFilter = Builders<Room>.Filter.Eq(r => r.Number, request.RoomId);
            var room = await _context.Rooms.Find(roomFilter).FirstOrDefaultAsync(cancellationToken);

            if (room == null)
            {
                room = new Room {Number = request.RoomId, CreationTime = DateTime.UtcNow };
                await _context.Rooms.InsertOneAsync(room, cancellationToken: cancellationToken);
            }
            
            _webSocketManager.AddConnection(request.RoomId, userSession.UserId.ToString(), request.WebSocket);
            await _webSocketManager.ReceiveMessagesAsync(request.WebSocket, request.RoomId, userSession.UserId.ToString());
            return Unit.Value; 
        }
    }
}