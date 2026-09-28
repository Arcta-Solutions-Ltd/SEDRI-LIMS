using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditPagesEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editpages',
                        Description: '@ConPag@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
