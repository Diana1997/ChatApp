using MediatR;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Interfaces;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IList<UserDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllUsersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.Find(_ => true).ToListAsync(cancellationToken);

            var userDtos = users.ConvertAll(user => new UserDto
            {
                Id = user.Id.ToString(),
                Nickname = user.Nickname
            });

            return userDtos;
        }
    }
}