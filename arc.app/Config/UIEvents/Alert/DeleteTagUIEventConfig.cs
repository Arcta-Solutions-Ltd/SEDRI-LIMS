using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteTagUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletetaguievent',
                        description: 'Delete Tag',
                        type: 'form',
                        action: 'deletetagform'
                    }";

            return newEvent;
        }
    }
}
