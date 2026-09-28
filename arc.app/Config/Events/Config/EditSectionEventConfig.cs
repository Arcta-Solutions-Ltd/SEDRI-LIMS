using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditSectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editSection',
                        Description: '@ConEdiN@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
