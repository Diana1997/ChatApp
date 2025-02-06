namespace Domain
{
    public class EventLog : BaseEntity
    {
        public ActionType ActionType { get; set; }
        public string Details { get; set; }
        public string UserId { get; set; } 
        public string RequestMethod { get; set; } 
        public string RequestPath { get; set; }
        public string ResponseStatus { get; set; }
    }
} 