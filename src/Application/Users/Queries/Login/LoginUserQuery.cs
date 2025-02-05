using System;
using MediatR;

namespace Application.Users.Queries.Login
{
    public class LoginUserQuery : IRequest<Guid>
    {
        public string Nickname { get; set; }
    }
}