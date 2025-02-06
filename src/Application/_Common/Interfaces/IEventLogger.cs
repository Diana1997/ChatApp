using System.Threading.Tasks;
using Application._Common.Models;

namespace Application._Common.Interfaces
{
    public interface IEventLogger
    {
        Task LogEvent(EventLogModel log);
    }
}