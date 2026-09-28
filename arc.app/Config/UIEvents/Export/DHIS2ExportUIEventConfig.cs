using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DHIS2ExportUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'dhis2export',
                        description: 'Configure and issue a DHIS2 export',
                        type: 'form',
                        action: 'dhis2exportform'
                    }";

            return newEvent;
        }
    }
}
