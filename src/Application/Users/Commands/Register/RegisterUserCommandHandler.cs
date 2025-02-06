using System;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Domain;
using MediatR;

namespace Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAppDbContext _context;

        public RegisterUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async  Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Nickname = request.Nickname,
                CreationTime = DateTime.UtcNow
            };
           await _context.Users.InsertOneAsync(user, cancellationToken: cancellationToken);
           return user.Id.ToString();
        }
    }
}