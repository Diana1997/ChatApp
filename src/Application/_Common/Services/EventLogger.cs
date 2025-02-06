using System.Threading.Tasks;
using Application._Common.Interfaces;
using Application._Common.Models;
using Domain;

namespace Application._Common.Services
{
    public class EventLogger : IEventLogger
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentDateTime _currentDateTime;

        public EventLogger(
            IAppDbContext context, 
            ICurrentDateTime currentDateTime)
        {
            _context = context;
            _currentDateTime = currentDateTime;
        }

      public async Task LogEvent(EventLogModel model)
        {
            var log = new EventLog
            {
                CreationTime = _currentDateTime.Now,
                ActionType = model.ActionType,
                Details = model.Details,
                UserId = model.UserId,
                RequestMethod = model.RequestMethod,
                RequestPath = model.RequestPath,
                ResponseStatus = model.ResponseStatus,

            };
            await _context.EventLogs.InsertOneAsync(log);
        }
    }
}