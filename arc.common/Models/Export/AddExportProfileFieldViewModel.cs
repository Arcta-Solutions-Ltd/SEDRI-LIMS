using Newtonsoft.Json;

namespace arc.common.Models.Export
{
    public class AddExportProfileFieldViewModel
    {
        public string Name { get; set; }
        public string Heading { get; set; }
        [JsonProperty("Id")]
        public int ExportProfileId { get; set; }
        public string CommentType { get; set; }
        public string CommentFormat { get; set; }
        public string Mapping {  get; set; }
        public string Antibiotic {  get; set; }
        public string TestMethod {  get; set; }
        public string TestMethodPrecedence {  get; set; }

    }
}
