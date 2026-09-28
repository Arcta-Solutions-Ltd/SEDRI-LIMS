using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SynonymsForOrganismQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'synonymsfororganismquery',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
