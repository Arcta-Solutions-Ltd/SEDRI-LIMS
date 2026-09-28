using System.Collections.Generic;

namespace arc.domain.Configuration.PagesConfig
{
    public class ConditionConfig
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string Comparison { get; set; }

        internal bool Evaluate(Dictionary<string, string> fields)
        {
            var value = fields[Field];

            switch (Comparison)
            {
                case "=":
                    if (value != Value)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
