using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddFormEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addForm',
                        Description: '@ConAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
