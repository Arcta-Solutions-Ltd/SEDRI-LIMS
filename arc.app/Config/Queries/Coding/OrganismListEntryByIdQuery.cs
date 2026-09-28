using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismListEntryByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrganismListEntryById',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
