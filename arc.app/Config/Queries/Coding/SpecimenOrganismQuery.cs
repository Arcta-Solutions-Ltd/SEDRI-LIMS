using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenOrganismQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenOrganism',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
