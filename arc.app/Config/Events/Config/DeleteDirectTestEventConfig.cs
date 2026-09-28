using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteDirectTest',
                        Description: '@ConDelB@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration',
                        DataRules: [
                            { type: 'NoRecord', query: 'directtestusagecountquery', message: '@ConYouA@' }
                        ]
                    }";
        }
    }
}
