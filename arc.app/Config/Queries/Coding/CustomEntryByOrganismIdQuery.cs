using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CustomEntryByOrganismIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'customentrybyorganismid',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
