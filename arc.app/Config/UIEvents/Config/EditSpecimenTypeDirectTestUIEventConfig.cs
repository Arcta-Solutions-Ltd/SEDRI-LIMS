using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSpecimenTypeDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editspecimentypedirecttestuievent',
                        description: 'Edit specimen type direct test default',
                        type: 'form',
                        action: 'editspecimentypedirecttestform'
                    }";

            return newEvent;
        }
    }
}
