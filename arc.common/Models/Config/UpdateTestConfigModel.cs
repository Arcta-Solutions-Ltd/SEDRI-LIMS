namespace arc.common.Models.Config
{
    public class UpdateTestConfigModel
    {
        public string Id { get; set; }
        public string TestToCloneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Enabled { get; set; }
    }
}
