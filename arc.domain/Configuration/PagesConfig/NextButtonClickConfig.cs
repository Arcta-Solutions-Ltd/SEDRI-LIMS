using System.Collections.Generic;

namespace arc.domain.Configuration.PagesConfig
{
    /// <summary>
    /// Represents the workflow state a page button applies when it is clicked, together with the
    /// rules that must evaluate to true before the state is applied.
    /// </summary>
    public class NextButtonClickConfig
    {
        /// <summary>
        /// Gets or sets the workflow state token added to the form state when the button is clicked.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the rules that must pass for the state to be applied. Each rule's Effect must
        /// match <see cref="State"/> for it to be evaluated.
        /// </summary>
        public List<RuleConfig> Rules { get; set; }

        /// <summary>
        /// Gets or sets whether this state may be created or changed by a user through the page rules
        /// configuration tool. 'Yes' makes the state user configurable; any other value (including null)
        /// marks the state as system owned and read only.
        /// </summary>
        public string Configurable { get; set; }
    }
}
