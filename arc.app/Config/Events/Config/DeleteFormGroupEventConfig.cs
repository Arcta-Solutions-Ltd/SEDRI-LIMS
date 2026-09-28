using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for deleting a form group.
    /// </summary>
    internal class DeleteFormGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'deleteformgroup',
                        Description: '@ConDelFG@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
