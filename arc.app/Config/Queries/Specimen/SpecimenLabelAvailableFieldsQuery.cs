using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenLabelAvailableFieldsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenLabelAvailableFields',
                        'TableName': 'Specimen',
                        'Type': 'Special',
                        'ResultMapping': 'specimenlabelavailablefieldsmapper'
                    }";
        }
    }
}
