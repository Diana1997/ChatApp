using Domain;

namespace Application._Common.Models
{
    public class EventLogModel
    {
        public EventLogModel(){}

        public EventLogModel(ActionType actionType, string details, string userId = null)
        {
            ActionType = actionType;
            Details = details;
            UserId = userId;
        }
        public ActionType ActionType { get; set; }
        public string Details { get; set; }
        public string UserId { get; set; } 
        public string RequestMethod { get; set; } 
        public string RequestPath { get; set; } 
        public string ResponseStatus { get; set; }
    }
}