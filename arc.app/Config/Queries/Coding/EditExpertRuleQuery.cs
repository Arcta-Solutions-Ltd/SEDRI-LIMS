using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditExpertRuleQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'editexpertrulequery', 'TableName': 'ExpertRule', 'Type': 'Special'
            }";
        }
    }
}
