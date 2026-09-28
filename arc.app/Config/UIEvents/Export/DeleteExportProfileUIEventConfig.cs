using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    internal class DeleteExportProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteexportprofileuievent',
                        description: 'Delete Export profile',
                        type: 'form',
                        action: 'deleteexportprofileform'
                    }";

            return newEvent;
        }
    }
}
