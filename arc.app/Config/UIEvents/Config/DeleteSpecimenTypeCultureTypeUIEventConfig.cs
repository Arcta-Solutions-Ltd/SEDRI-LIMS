using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteSpecimenTypeCultureTypeUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletespecimentypeculturetypeuievent',
                        description: 'Delete specimen type culture type default',
                        type: 'form',
                        action: 'deletespecimentypeculturetypeform'
                    }";

            return newEvent;
        }
    }
}
