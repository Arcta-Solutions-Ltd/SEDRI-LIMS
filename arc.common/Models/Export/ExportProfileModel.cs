using Newtonsoft.Json;
using System;

namespace arc.common.Models.Export
{
    public class ExportProfileModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        public string IncludeHeaderRow { get; set; }
        public int TableNameId { get; set; }
    }

}
