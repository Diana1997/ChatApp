using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using MediatR;
using MongoDB.Driver;

namespace Application.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, IList<RoomDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllRoomsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async  Task<IList<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms.Find(_ => true).ToListAsync(cancellationToken);

            var roomDtos = rooms.ConvertAll(room => new RoomDto
            {
                Id = room.Id.ToString(),
            });

            return roomDtos;
        }
    }
}