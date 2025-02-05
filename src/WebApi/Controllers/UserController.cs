using System;
using System.Net;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Commands.Register;
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
            var id = await Mediator.Send(new RegisterUserCommand()
            {
                Nickname = name
            });
           return Ok(id);
        }
    }
}