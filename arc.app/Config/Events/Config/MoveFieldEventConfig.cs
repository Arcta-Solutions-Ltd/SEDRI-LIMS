using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for moving a field to another form group (including across pages).
    /// </summary>
    internal class MoveFieldEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'movefield',
                        Description: '@ConMovFG@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
