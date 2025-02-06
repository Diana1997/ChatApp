namespace Domain
{
    public class WebSocketConnection : BaseEntity
    { 
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string WebSocketId { get; set; }
    }
}