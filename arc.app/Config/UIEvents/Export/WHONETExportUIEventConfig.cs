using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class WHONETExportUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'whonetexport',
                        description: 'Configure and issue a WHONET export',
                        type: 'form',
                        action: 'whonetexportform'
                    }";

            return newEvent;
        }
    }
}
