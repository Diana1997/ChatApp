using MediatR;

namespace Application.Users.Commands.Register
{
    public class RegisterUserCommand : IRequest<string>
    {
        public string Nickname { get; set; }
    }
}