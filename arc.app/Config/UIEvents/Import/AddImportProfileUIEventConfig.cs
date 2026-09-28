using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class AddImportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addimportprofileuievent',
                        description: 'Add new import profile',
                        type: 'form',
                        action: 'addimportprofileform'
                    }";

            return newEvent;
        }
    }
}
