using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class DeleteImportProfileFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteimportprofilefielduievent',
                        description: 'Delete an existing import profile field',
                        type: 'form',
                        action: 'deleteimportprofilefieldform'
                    }";

            return newEvent;
        }
    }
}
