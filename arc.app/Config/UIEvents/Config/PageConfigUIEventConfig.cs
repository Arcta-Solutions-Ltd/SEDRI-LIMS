using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PageConfigUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'pageconfiguievent',
                        description: 'Page config ui event',
                        type: 'view-record',
                        action: 'formConfig'
                    }";

            return newEvent;
        }
    }
}
