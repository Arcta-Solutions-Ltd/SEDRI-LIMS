using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SubSpeciesListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SubSpeciesList',
                        'TableName': 'SubSpecies',
                        'Type': 'Special'
                    }";
        }
    }
}
