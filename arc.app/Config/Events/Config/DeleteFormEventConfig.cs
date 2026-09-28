using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteFormEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteForm',
                        Description: '@ConDel@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
