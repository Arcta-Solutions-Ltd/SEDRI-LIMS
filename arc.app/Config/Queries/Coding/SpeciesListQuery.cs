using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpeciesListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpeciesList',
                        'TableName': 'Species',
                        'Type': 'Special'
                    }";
        }
    }
}
