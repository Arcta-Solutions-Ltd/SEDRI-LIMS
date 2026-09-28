using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    internal class AddExportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addexportprofileuievent',
                        description: 'Add new export profile',
                        type: 'form',
                        action: 'addexportprofileform'
                    }";

            return newEvent;
        }
    }
}
