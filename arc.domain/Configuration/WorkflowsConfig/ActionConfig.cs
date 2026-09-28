using System.Collections.Generic;

namespace arc.domain.Configuration.WorkflowsConfig
{
    public class ActionConfig
    {
        public string Default { get; set; }
        public List<ActionConditionConfig> Options { get; set; } = new List<ActionConditionConfig>();
    }
}
