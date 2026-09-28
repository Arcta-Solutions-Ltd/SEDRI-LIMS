using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class SpecimenTypeCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'specimentypecount',
                        'TableName': 'Specimen',
                        'Type': 'Special'
                    }";
        }
    }
}
