using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.ChatMessages.Queries.GetAll
{
    public class GetAllChatMessagesQueryHandler : IRequestHandler<GetAllChatMessagesQuery, IList<ChatMessageDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllChatMessagesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<ChatMessageDto>> Handle(GetAllChatMessagesQuery request, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(request.RoomId, out ObjectId parsedRoomId))
            {
                throw new BadRequestException("Invalid Room ID format."); 
            }
            
            var messages = await _context.ChatMessages
                .Find(msg => msg.RoomId ==parsedRoomId)
                .ToListAsync(cancellationToken);

            var messageDtos = messages.ConvertAll(msg => new ChatMessageDto
            {
                Id = msg.Id.ToString(),
                RoomId = msg.RoomId.ToString(),
                UserId = msg.UserId.ToString(),
                Message = msg.Message,
                CreationTime = msg.CreationTime 
            });
            return messageDtos;
        }
    }
}