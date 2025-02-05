using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Rooms.Queries.GetAllRooms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class RoomsController : ApiController
    {
        [ProducesResponseType(typeof(IList<RoomDto>), StatusCodes.Status200OK)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await Mediator.Send(new GetAllRoomsQuery());
            return Ok(rooms); 
        }
    }
}