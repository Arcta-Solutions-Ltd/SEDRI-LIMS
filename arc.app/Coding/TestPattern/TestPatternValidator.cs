using arc.app.Common;
using arc.common.Models.Coding;
using Newtonsoft.Json;

namespace arc.app.Coding
{
    internal class TestPatternValidator : ISpecialValidator
    {
        private readonly string _message;

        public TestPatternValidator(string message)
        {
            _message = message;
        }

        public string ValidateMessage()
        {
            var testPattern = JsonConvert.DeserializeObject<TestPatternWithoutCraftedModel>(_message);

            if (testPattern.AntibioticGrid == null || testPattern.AntibioticGrid.Count == 0)
            {
                return "@TesAddI@";
            }

            foreach (var testpatternLine in testPattern.AntibioticGrid)
            {
                if (testpatternLine.GuidelinesId == 0)
                {
                    return "@TesAddJ@";
                }
            }
            return "";
        }
    }
}
