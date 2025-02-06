using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Domain;
using MongoDB.Bson;

namespace Application._Common.Services
{
    public class WebSocketManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _connections;
        private readonly IAppDbContext _context;

        public WebSocketManager(IAppDbContext context)
        {
            _connections = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
            _context = context;
        }

        public void AddConnection(string roomId, string userId, WebSocket webSocket)
        {
            var roomConnections = _connections.GetOrAdd(roomId, _ => new ConcurrentDictionary<string, WebSocket>());
            roomConnections[userId] = webSocket;
        }

        public async Task ReceiveMessagesAsync(WebSocket webSocket, string roomId, string userId)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await RemoveConnectionAsync(roomId, userId);
                    break;
                }

                var messageText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await ProcessMessageAsync(roomId, userId, messageText);
            }
        }

        private async Task ProcessMessageAsync(string roomId, string senderId, string messageText)
        {
            messageText = System.Text.RegularExpressions.Regex.Replace(messageText, @"\s{2,}", " ");

            var messageParts = messageText.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (messageParts.Length < 2)
            {
                return;
            }

            var command = messageParts[0].ToUpper();
            var targetIdOrMessage = messageParts[1];
            var messageBody = messageParts.Length == 3 ? messageParts[2] : "";

            switch (command)
            {
                case "PUSH":
                    await BroadcastMessageAsync(roomId, senderId, targetIdOrMessage);
                    return;
                case "PUB":
                    await SendPrivateMessageAsync(senderId, targetIdOrMessage, messageBody);
                    return;
            }
        }

        private async Task BroadcastMessageAsync(string roomId, string senderId, string message)
        {
            if (!_connections.TryGetValue(roomId, out var users)) return;

            var chatMessage = new ChatMessage
            {
                RoomId = roomId,
                UserId = ObjectId.Parse(senderId),
                Message = message,
                CreationTime = DateTime.UtcNow
            };

            await _context.ChatMessages.InsertOneAsync(chatMessage);

            var data = Encoding.UTF8.GetBytes($"{senderId}: {message}");
            var buffer = new ArraySegment<byte>(data);

            foreach (var (userId, webSocket) in users)
            {
                if (userId != senderId && webSocket.State == WebSocketState.Open)
                {
                    await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task SendPrivateMessageAsync(string senderId, string targetId, string message)
        {
            foreach (var room in _connections.Values)
            {
                if (room.TryGetValue(targetId, out var webSocket) && webSocket.State == WebSocketState.Open)
                {
                    var privateMessage = Encoding.UTF8.GetBytes($"Private from {senderId}: {message}");
                    var buffer = new ArraySegment<byte>(privateMessage);

                    await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
                }
            }
        }

        private async Task RemoveConnectionAsync(string roomId, string userId)
        {
            if (_connections.TryGetValue(roomId, out var users) && users.TryRemove(userId, out var webSocket))
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server",
                        CancellationToken.None);
                }
            }
        }
    }
}