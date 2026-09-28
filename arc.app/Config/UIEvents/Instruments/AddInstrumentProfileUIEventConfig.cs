

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    public class AddInstrumentProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addinstrumentprofileuievent',
                        description: '@InsAddC@',
                        type: 'form',
                        action: 'addinstrumentprofileform'
                    }";

            return newEvent;
        }
    }
}
