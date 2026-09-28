using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// Represents a single column definition within a fieldgrid field configuration.
    /// </summary>
    public class GridDefinitionModel
    {
        public string GridId { get; set; }
        /// <summary>
        /// Optional header text displayed above the grid column when the form is rendered.
        /// </summary>
        public string GridTitle { get; set; }
        public string GridType { get; set; }
        public string GridWidth { get; set; }
        public string GridOption { get; set; }
        public string MultiSelect { get; set; }
        /// <summary>
        /// Visibility rules for the grid column. Structure matches FormGroupRuleModel: Effect, Field, Rule, Value.
        /// </summary>
        public List<FormGroupRuleModel> Rules { get; set; }
    }
}
