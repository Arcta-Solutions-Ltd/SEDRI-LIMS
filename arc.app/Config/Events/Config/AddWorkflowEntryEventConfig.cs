using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Workflow Entry" event.
/// </summary>
internal class AddWorkflowEntryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow Entry" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the attributes for the "Add Workflow Entry" event,
    /// including the event name, description, type, topic, table name, and validation rules.
    /// The validation rules ensure required fields, such as "EventField" and "EntryStates,"
    /// are properly validated, with corresponding error messages.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, type, topic, table name, and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'addworkflowentry', 
                        Description: '@ConAddQ@',
                        EventType : 'specialadddata', 
                        Topic : 'Configuration',
                        TableName: 'Configs',
                        ValidationRules: [
                            { field: 'EventField', rule: 'required', message: '@ConAne@'},
                            { field: 'EntryStates', rule: 'required', message: '@ConAneA@'}
                        ],
                    }";
    }
}
