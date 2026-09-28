using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for <see cref="arc.app.Common.SpecialFactory"/> home dashboard Recently Used (filteredget).
    /// </summary>
    internal class HomeDashboardRecentlyUsedQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'homedashboardrecentlyused',
                        'TableName': 'Specimen',
                        'Type': 'Special'
                    }";
        }
    }
}
