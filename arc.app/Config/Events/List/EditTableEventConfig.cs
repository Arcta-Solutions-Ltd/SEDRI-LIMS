using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Provides the event configuration definition for the "Edit Table" (edit list) operation.
    /// </summary>
    internal class EditTableEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string that defines the event configuration for editing an existing table.
        /// Validation requires the Description field (displayed to the user as "Name") to be
        /// populated; the message tag <c>@TabAA@</c> resolves to "A table name must be entered".
        /// </summary>
        /// <returns>
        /// A JSON string containing the event name, type, topic, table name, mapper reference,
        /// and validation rules.
        /// </returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'editTable', 
                        Description: '@TabEdiC@',
                        EventType : 'editdata', 
                        Topic : 'Lists', 
                        TableName: 'List',
                        Mapping: 'edittablemapper',
                        ValidationRules: [
                            { field: 'Description', rule: 'required', message: '@TabAA@'}
                        ]
                    }";
        }
    }
}
