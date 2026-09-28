using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteMenuOptionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletemenuoptionuievent',
                        description: 'Delete menu option',
                        type: 'form',
                        action: 'deletemenuoptionform'
                    }";

            return newEvent;
        }
    }
}
