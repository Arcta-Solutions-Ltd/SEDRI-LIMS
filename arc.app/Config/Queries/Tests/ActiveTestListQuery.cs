using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ActiveTestListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'activetestlistquery', 'TableName': 'Tests', 'Type': 'Special', 
                        'ResultMapping': 'activetestlistresultmapper',
                        'Translate': true
                     }";
        }
    }
}

