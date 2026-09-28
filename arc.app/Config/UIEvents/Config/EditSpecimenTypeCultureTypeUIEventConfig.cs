using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSpecimenTypeCultureTypeUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editspecimentypeculturetypeuievent',
                        description: 'Edit specimen type culture type default',
                        type: 'form',
                        action: 'editspecimentypeculturetypeform'
                    }";

            return newEvent;
        }
    }
}
