using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteSpecimenTypeDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletespecimentypedirecttestuievent',
                        description: 'Delete specimen type direct test default',
                        type: 'form',
                        action: 'deletespecimentypedirecttestform'
                    }";

            return newEvent;
        }
    }
}
