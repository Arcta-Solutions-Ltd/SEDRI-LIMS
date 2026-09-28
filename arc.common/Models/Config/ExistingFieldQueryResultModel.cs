using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// Result of <c>existingfieldlistquery</c>, the initial query behind the Add Existing Field form.
    /// </summary>
    public class ExistingFieldQueryResultModel
    {
        /// <summary>
        /// Gets or sets the target context id echoed back to the save payload.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the translated target page title, shown as read only context.
        /// </summary>
        public string TargetPageTitle { get; set; }

        /// <summary>
        /// Gets or sets the resolved target entity table, or null when the page is unscoped.
        /// </summary>
        public string TargetTable { get; set; }

        /// <summary>
        /// Gets or sets the selectable options. Consumed by the page field
        /// <c>ExistingFieldIds</c> whose <c>optionsName</c> is <c>existingFieldOptions</c>.
        /// </summary>
        public List<ExistingFieldOptionModel> ExistingFieldOptions { get; set; } = [];
    }
}
