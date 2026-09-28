using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddSpecimenTypeDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addspecimentypedirecttestuievent',
                        description: 'Add specimen type direct test default',
                        type: 'form',
                        action: 'addspecimentypedirecttestform'
                    }";

            return newEvent;
        }
    }
}
