using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CultureTestUsageCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'culturetestusagecountquery', 'TableName': 'Tests', 'Type': 'Special'
                    }";
        }
    }
}
