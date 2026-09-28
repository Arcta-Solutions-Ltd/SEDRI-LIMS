using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event definition that opens the page visibility and state configuration form from the
    /// page header in Define Page Contents.
    /// </summary>
    internal class EditPageRulesUIEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the edit page rules UI event.
        /// </summary>
        /// <returns>A JSON formatted UI event definition.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpagerulesuievent',
                        description: 'Edit page rules',
                        type: 'form',
                        action: 'editpagerulesform'
                    }";

            return newEvent;
        }
    }
}
