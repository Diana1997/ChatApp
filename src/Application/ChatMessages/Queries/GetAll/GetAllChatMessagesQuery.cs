using System.Collections.Generic;
using MediatR;
using MongoDB.Bson;

namespace Application.ChatMessages.Queries.GetAll
{
    public class GetAllChatMessagesQuery : IRequest<IList<ChatMessageDto>>
    {
        public GetAllChatMessagesQuery(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; set; }
    }
}