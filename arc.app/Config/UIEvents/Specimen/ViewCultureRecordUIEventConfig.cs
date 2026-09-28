using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewCultureRecordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewculturerecord',
                        description: 'View culture record',
                        type: 'view-record',
                        action: 'cultures'
                    }";

            return newEvent;
        }
    }
}
