using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for moving a form group to another page within the same form.
    /// </summary>
    internal class MoveFormGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'moveformgroup',
                        Description: '@ConMovFGGrp@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
