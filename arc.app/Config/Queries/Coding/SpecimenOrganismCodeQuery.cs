using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenOrganismCodeQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenOrganismCode',
                        'TableName': 'Organism',
                        'Type': 'Special'
                    }";
        }
    }
}
