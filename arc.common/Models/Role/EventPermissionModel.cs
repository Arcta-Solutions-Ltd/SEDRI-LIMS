namespace arc.common.Models.Role
{
    public class EventPermissionModel
    {
        public string Key { get; set; }
        public string Event { get; set; }
        public string Topic { get; set; }
        public string TranslatedTopic { get; set; }
        public string Allowed { get; set; }
        public string Description { get; set; }
    }
}
