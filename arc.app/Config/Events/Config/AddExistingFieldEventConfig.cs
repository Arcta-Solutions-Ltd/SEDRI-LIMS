using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for adding fields that already exist on other forms to a page by
    /// reference.
    /// </summary>
    internal class AddExistingFieldEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration string for the add existing field event.
        /// </summary>
        /// <returns>A JSON string defining the add existing field event.</returns>
        public string Get()
        {
            return @"{
                        EventName: 'addexistingfield',
                        Description: '@ConAddEF@',
                        EventType: 'special',
                        TableName: 'configs',
                        Topic: 'Configuration',
                        ValidationRules: [
                            { field: 'ExistingFieldIds', rule: 'required', message: '@ConExiFF@' }
                        ]
                    }";
        }
    }
}
