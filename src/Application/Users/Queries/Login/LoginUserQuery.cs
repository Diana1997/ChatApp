using MediatR;

namespace Application.Users.Queries.Login
{
    public class LoginUserQuery : IRequest<string>
    {
        public string Nickname { get; set; }
    }
}