using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editCultureTest',
                        Description: '@ConEdiA@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
