using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenTagCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'specimentagcount',
                        'TableName': 'Specimen',
                        'Type': 'Special'
                    }";
        }
    }
}
