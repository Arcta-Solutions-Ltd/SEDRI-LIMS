using arc.app.Common;

namespace arc.app.Config.Queries.Reports
{
    internal class ResistantAntibioticQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'resistantantibioticquery', 'TableName': 'Specimen', 'Type': 'Special'
             }";
        }
    }
}
