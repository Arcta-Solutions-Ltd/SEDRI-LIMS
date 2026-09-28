using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for saving the visibility rule and state definition of a page.
    /// </summary>
    internal class EditPageRulesEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the edit page rules event.
        /// </summary>
        /// <returns>A JSON formatted event definition.</returns>
        public string Get()
        {
            return @"{
                        EventName: 'editpagerules',
                        Description: '@ConPagVis@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
