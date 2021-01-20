namespace Chat.Data.Entities
{
    public class Conversation : BaseEntity
    {
        public int SenderId { get; set; }
        public string Message { get; set; }
        public int? ReceiverId { get; set; }
        public int? GroupId { get; set; }
        public virtual User Sender { get; set; }
    }
}