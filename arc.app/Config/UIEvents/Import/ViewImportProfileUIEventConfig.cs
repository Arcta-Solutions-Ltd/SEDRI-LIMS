using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class ViewImportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewimportprofileuievent',
                        description: 'View import profile record',
                        type: 'view-record',
                        action: 'importprofile'
                    }";

            return newEvent;
        }
    }
}
