using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteCultureTestConfigEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteculturetestconfig',
                        Description: '@ConDelA@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration',
                        DataRules: [
                            { type: 'NoRecord', query: 'culturetestusagecountquery', message: '@ConYouA@' }
                        ]
                    }";
        }
    }
}
