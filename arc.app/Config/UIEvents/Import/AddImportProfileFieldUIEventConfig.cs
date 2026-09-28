using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class AddImportProfileFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addimportprofilefielduievent',
                        description: 'Add new field to import profile',
                        type: 'form',
                        action: 'addimportprofilefieldform'
                    }";

            return newEvent;
        }
    }
}
