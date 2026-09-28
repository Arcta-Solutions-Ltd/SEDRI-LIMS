using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class EditImportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editimportprofileuievent',
                        description: 'Edit Import Profile',
                        type: 'form',
                        action: 'editimportprofileform'
                    }";

            return newEvent;
        }
    }
}
