using System.Threading.Tasks;
using Application.ChatMessages.Commands.ConnectToChat;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WebSocketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SessionAuthorize]
        [HttpGet("{roomId}/{sessionId}")]
        public async Task Connect(string roomId, string sessionId)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = 400; 
                return;
            }

            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            await _mediator.Send(new ConnectToChatCommand
            {
                RoomId = roomId,
                SessionId = sessionId,
                WebSocket = webSocket
            });
        }
    }
}