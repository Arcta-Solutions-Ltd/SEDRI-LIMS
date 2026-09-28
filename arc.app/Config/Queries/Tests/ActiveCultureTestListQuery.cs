using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ActiveCultureTestListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'activeculturetestlistquery', 'TableName': 'CultureTests', 'Type': 'Special', 
                        'ResultMapping': 'activetestlistresultmapper',
                        'Translate': true
                     }";
        }
    }
}
