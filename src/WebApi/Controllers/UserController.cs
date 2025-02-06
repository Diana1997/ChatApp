using System.Collections.Generic;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Application._Common.Models;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.Login;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;

namespace WebApi.Controllers
{

    public class UserController : ApiController
    {
        private readonly IEventLogger _eventLogger;

        public UserController(IEventLogger eventLogger)
        {
            _eventLogger = eventLogger;
        }

        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("register/{name}")]
        public async Task<IActionResult> Register(string name)
        {
            var id = await Mediator.Send(new RegisterUserCommand
            {
                Nickname = name
            }); 
            
            
            await _eventLogger.LogEvent(
                new EventLogModel(ActionType.UserRegistered, name, id));
           return Ok(id);
        }

        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            var sessionId = await Mediator.Send(query);
                  
            await _eventLogger.LogEvent(
                new EventLogModel(ActionType.UserLogin, query.Nickname));
            
            return Ok(sessionId);
        }
        
        [ProducesResponseType(typeof(IList<UserDto>), StatusCodes.Status200OK)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await Mediator.Send(new GetAllUsersQuery());
            return Ok(users); 
        }
    }
}