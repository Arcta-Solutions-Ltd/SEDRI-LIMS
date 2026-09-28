using System.Collections.Generic;

namespace arc.domain.Configuration.EventsConfig
{
    public class ValidationRuleConfig
    {
        public string Field { get; set; }
        public string Rule { get; set; }
        public IEnumerable<ConditionConfig> Conditions { get; set; }
        public string Comparison { get; set; }
        public string Message { get; set; }

        private Dictionary<string, string> _fields;

        internal string Evaluate(Dictionary<string,string> fields) 
        {
            _fields = new Dictionary<string, string>();
            foreach (var field in fields)
            {
                if (! _fields.ContainsKey(field.Key.ToLower()))
                {
                    _fields.Add(field.Key.ToLower(), field.Value);
                }
            }

            switch (Rule.ToLower())
                {
                    case "required":
                        return EvaluateRequiredRule();
                    case "valuecompare":
                        return EvaluateValueCompareRule();
                }

            return "";
        }

        private string EvaluateRequiredRule()
        {
            var errorMessage = "";
            
            if (_fields.ContainsKey(Field.ToLower()))
            {
                var value = _fields[Field.ToLower()];

                if (string.IsNullOrEmpty(value))
                {
                    if (AreConditionsMet())
                    {
                        errorMessage = Message;
                    }
                }
            } else
            {
                errorMessage = Message;
            }

            return errorMessage;
        }

        private string EvaluateValueCompareRule()
        {
            //var valueOne = GetValue(FirstValue);
            //var valueTwo = GetValue(SecondValue);

            //switch (Comparison)
            //{
            //    case "=":
            //        if (valueOne != valueTwo) { return Message; }
            //        break;
            //    case "!=":
            //        if (valueOne == valueTwo) { return Message; }
            //        break;
            //}
            return "";
        }

        private string GetValue(string value)
        {
            if (value.StartsWith("#"))
            {
                var fieldname = value.Remove(0, 1).ToLower();
                return _fields[fieldname];
            } else
            {
                return value;
            }
        }

        private bool AreConditionsMet()
        {
            var conditionsMet = true;
            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    if (!condition.Evaluate(_fields))
                    {
                        conditionsMet = false;
                    }
                }
            }
            return conditionsMet;
        }

    }
}


