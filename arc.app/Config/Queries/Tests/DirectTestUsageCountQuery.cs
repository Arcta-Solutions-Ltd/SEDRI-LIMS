using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DirectTestUsageCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'directtestusagecountquery', 'TableName': 'Tests', 'Type': 'Special'
                    }";
        }
    }
}
