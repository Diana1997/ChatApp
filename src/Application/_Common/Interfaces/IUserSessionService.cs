using System;
using System.Threading.Tasks;
using Domain;

namespace Application._Common.Interfaces
{
    public interface IUserSessionService
    {
        bool ExistsActiveSession(string sessionId);
     
    }
}