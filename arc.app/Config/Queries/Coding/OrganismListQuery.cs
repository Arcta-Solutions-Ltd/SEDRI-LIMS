using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrganismList',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
