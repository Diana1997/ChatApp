using System.Collections.Generic;
using MediatR;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IList<UserDto>>
    {
        
    }
}