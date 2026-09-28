using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ActiveTestByIdForTestListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'activetestbyidfortestlistquery', 'TableName': 'Tests', 'Type': 'Special', 
                        'ResultMapping': 'testlistitemresultmapper',
                        'Translate': true
                     }";
        }
    }
}
