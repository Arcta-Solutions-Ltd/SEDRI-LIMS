using Newtonsoft.Json;

namespace arc.domain.Configuration.ViewConfig.ListViewConfig
{
    public class FilterConfig
    {
        public string Key { get; set; }
        public string PlaceHolder { get; set; }
        public  bool MultiSelect { get; set; }
        public short Width { get; set; }
        public string OptionsName { get; set; }
        public string FieldName { get; set; }
        public bool Dynamic { get; set; }
        public bool IncludeFixed { get; set; }
        public string Type { get; set; }
        public int DropDownWidth { get; set; }

        /// <summary>
        /// When false, suppresses in-place add actions on hierarchical picker filters
        /// (e.g. LocationList, OrganisationList). Defaults to false in FilterHierarchyPicker when omitted.
        /// </summary>
        [JsonProperty("allowAdd")]
        public bool? AllowAdd { get; set; }
    }
}
