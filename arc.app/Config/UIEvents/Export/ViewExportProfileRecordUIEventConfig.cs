using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    internal class ViewExportProfileRecordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewexportprofilerecorduievent',
                        description: 'View export profile record',
                        type: 'view-record',
                        action: 'exportprofile'
                    }";

            return newEvent;
        }
    }
}
