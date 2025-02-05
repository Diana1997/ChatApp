using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.Login;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WebApi.Controllers
{

    public class UserController : ApiController
    {
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost("register/{name}")]
        public async Task<IActionResult> Register(string name)
        {
            var id = await Mediator.Send(new RegisterUserCommand
            {
                Nickname = name
            });
           return Ok(id);
        }

        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(string name)
        {
            var sessionId = await Mediator.Send(new LoginUserQuery
            {
                Nickname = name
            });
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