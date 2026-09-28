using System.Collections.Generic;
using Newtonsoft.Json;

namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Describes a side-effect action tied to a workflow state transition option (e.g. publishing a report).
    /// </summary>
    public class ActionConditionConfig
    {
        /// <summary>
        /// Name of the action implementation to run (e.g. publish report); matched case-insensitively by the action factory.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Key-value arguments passed into the action, serialized as a JSON object in workflow configuration.
        /// For report publishing the canonical keys are <c>reportconfig</c> (report configuration name) and
        /// <c>reportname</c> (language key token such as <c>@RepFin@</c>). Values must be stable identifiers or
        /// keys, not translated display text.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

        public string NewState { get; set; }
        public List<WorkflowConditionConfig> Conditions { get; set; }
        public string ConditionType { get; set; }

        /// <summary>
        /// Returns whether this option's <see cref="Conditions"/> are satisfied for the given state and event message.
        /// </summary>
        public bool IsConditionMatched(string currentState, string message)
        {
            if (Conditions == null) return true;
            foreach (var condition in Conditions)
            {
                var matched = condition.IsConditionMatched(currentState, message);

                if (matched && ConditionType.ToLower() == "or")
                {
                    return true;
                }

                if (!matched && ConditionType.ToLower() == "and")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
