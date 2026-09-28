using System.Collections.Generic;

namespace arc.domain.Configuration.PagesConfig
{
    public class RuleConfig
    {
        public string Effect { get; set; }
        public string Field { get; set; }
        public string Rule { get; set; }
        public string Value { get; set; }
        public IEnumerable<ConditionConfig> Conditions { get; set; }
    }
}
