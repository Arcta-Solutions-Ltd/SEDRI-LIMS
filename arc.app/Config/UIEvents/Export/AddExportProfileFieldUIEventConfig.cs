using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    internal class AddExportProfileFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addexportprofilefielduievent',
                        description: 'Add new field to profile',
                        type: 'form',
                        action: 'addexportprofilefieldform'
                    }";

            return newEvent;
        }
    }
}
