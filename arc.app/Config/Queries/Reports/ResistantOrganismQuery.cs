using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ResistantOrganismQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'resistantorganismquery', 'TableName': 'Specimen', 'Type': 'Special'
                    }";
        }
    }
}
