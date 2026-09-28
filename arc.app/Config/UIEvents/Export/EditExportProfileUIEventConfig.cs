using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    internal class EditExportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editexportprofileuievent',
                        description: 'Edit Export Profile',
                        type: 'form',
                        action: 'editexportprofileform'
                    }";

            return newEvent;
        }
    }
}
