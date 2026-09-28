using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class DeleteImportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteimportprofileuievent',
                        description: 'Delete Import profile',
                        type: 'form',
                        action: 'deleteimportprofileform'
                    }";

            return newEvent;
        }
    }
}
