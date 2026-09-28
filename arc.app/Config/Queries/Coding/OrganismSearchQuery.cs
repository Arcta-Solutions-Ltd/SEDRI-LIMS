using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismSearchQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrganismSearch',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
