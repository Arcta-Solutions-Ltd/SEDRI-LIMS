using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ActiveCultureTestByIdForTestListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'activeculturetestbyidfortestlistquery', 'TableName': 'CultureTests', 'Type': 'Special', 
                        'ResultMapping': 'testlistitemresultmapper',
                        'Translate': true
                     }";
        }
    }
}
