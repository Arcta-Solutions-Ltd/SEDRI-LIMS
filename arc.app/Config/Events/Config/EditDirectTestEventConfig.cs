using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editDirectTest',
                        Description: '@ConEdiB@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
