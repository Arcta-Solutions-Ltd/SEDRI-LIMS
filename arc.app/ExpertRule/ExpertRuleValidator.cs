using arc.app.Common;
using Newtonsoft.Json;
using ExpertRuleModel = arc.domain.Coding.ExpertRule;

namespace arc.app.ExpertRule
{
    /// <summary>
    /// Validates expert rule data on add/edit. Actions are optional: an empty action grid means a
    /// message-only rule (surfaced as comment alerts on the AST page). Conditions and test conditions
    /// are optional (e.g. intrinsic resistance rules).
    /// </summary>
    internal class ExpertRuleValidator : ISpecialValidator
    {
        private readonly string _message;

        public ExpertRuleValidator(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Ensures the payload deserializes as an expert rule. There is no minimum action count.
        /// </summary>
        /// <returns>Empty string when valid; reserved for future validation error keys.</returns>
        public string ValidateMessage()
        {
            JsonConvert.DeserializeObject<ExpertRuleModel>(_message);
            return "";
        }
    }
}
