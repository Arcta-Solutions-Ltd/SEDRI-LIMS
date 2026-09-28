using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteLaboratoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletelaboratoryuievent',
                        description: 'Delete a laboratory',
                        type: 'form',
                        action: 'deletelaboratoryform'
                    }";

            return newEvent;
        }
    }
}
