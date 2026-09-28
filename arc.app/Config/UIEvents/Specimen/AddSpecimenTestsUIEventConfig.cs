using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddSpecimenTestsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addspecimentests',
                        description: 'Add specimen test',
                        type: 'form',
                        action: 'addspecimentestsform'
                    }";

            return newEvent;
        }
    }
}
