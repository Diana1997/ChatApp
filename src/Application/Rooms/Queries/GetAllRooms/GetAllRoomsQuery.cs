using System.Collections.Generic;
using MediatR;

namespace Application.Rooms.Queries.GetAllRooms
{
    public class GetAllRoomsQuery : IRequest<IList<RoomDto>>
    {
        
    }
}