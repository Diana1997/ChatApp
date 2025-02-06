using System.Net.WebSockets;
using MediatR;

namespace Application.ChatMessages.Commands.ConnectToChat
{
    public class ConnectToChatCommand : IRequest<Unit>
    {
        public string RoomId { get; set; }
        public string SessionId { get; set; }
        public WebSocket WebSocket { get; set; }
    }
}