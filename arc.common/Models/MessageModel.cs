namespace arc.common.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public int StatusId { get; set; }
        public int TopicId { get; set; }
        public int EventId { get; set; }
        /// <summary>Normalized lower-case table name from event config (see <c>queue.tablename</c>).</summary>
        public string TableName { get; set; }
    }
}
