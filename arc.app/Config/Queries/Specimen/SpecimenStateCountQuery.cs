using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenStateCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'specimenstatecount',
                        'TableName': 'Specimen',
                        'Type': 'Special'
                    }";
        }
    }
}
