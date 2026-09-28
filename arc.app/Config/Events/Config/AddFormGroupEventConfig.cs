using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for adding a form group to a page.
    /// </summary>
    internal class AddFormGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'addformgroup',
                        Description: '@ConAddFG@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
