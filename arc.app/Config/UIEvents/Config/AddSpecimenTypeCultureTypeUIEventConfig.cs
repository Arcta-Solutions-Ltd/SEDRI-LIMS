using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddSpecimenTypeCultureTypeUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addspecimentypeculturetypeuievent',
                        description: 'Add specimen type culture type default',
                        type: 'form',
                        action: 'addspecimentypeculturetypeform'
                    }";

            return newEvent;
        }
    }
}
