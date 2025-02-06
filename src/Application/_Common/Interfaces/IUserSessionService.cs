namespace Application._Common.Interfaces
{
    public interface IUserSessionService
    {
        bool ExistsActiveSession(string sessionId);
    }
}