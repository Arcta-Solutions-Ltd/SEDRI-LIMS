using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleOrganismForOrganismListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleOrganismForOrganismList',
                        'TableName': 'Genus',
                        'Type': 'Special'
                    }";
        }
    }
}
