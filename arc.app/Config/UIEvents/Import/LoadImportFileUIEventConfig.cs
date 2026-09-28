using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class LoadImportFileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'loadimportfileuievent',
                        description: 'Load import file',
                        type: 'form',
                        action: 'loadimportfileform'
                    }";

            return newEvent;
        }
    }
}
