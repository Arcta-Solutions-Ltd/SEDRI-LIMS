using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class HPyloriantigenTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'hpyloriantigentestuievent',
                        description: 'H. Pylori antigen Test',
                        type: 'form',
                        action: 'HPyloriantigentestform'
                    }";

            return newEvent;
        }
    }
}
