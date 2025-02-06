using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using MediatR;
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
            var messages = await _context.ChatMessages
                .Find(msg => msg.RoomId == request.RoomId)
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