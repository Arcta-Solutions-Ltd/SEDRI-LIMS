using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.common.Models.Export
{
    public  class UpdateExportProfileFieldsViewModel
    {
        public int Id { get; set; }
        public List<ExpProField> FieldList { get; set; }
    }
    public class ExpProField
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }

}
