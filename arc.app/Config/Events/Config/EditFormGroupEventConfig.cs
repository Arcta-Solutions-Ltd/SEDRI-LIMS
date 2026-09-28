using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for editing a form group.
    /// </summary>
    internal class EditFormGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'editformgroup',
                        Description: '@ConEdiFG@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
