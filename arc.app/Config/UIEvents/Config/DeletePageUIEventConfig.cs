using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeletePageUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletepageuievent',
                        description: 'Delete page definition',
                        type: 'form',
                        action: 'deletepageform'
                    }";

            return newEvent;
        }
    }
}
