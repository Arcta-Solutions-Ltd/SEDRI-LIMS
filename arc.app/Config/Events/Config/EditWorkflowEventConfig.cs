using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Workflow" event.
/// </summary>
internal class EditWorkflowEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Workflow" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the attributes for the "Edit Workflow" event, including:
    /// - EventName: Specifies the name of the event.
    /// - Description: Provides a brief explanation of the event's purpose.
    /// - EventType: Indicates the type of event, here labeled as "special."
    /// - Topic: Identifies the context of the event, which is "Configuration."
    /// - TableName: Specifies the name of the associated database table, "Configs."
    /// While the configuration includes an array for validation rules, none are specified in this implementation.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, event type, topic, table name, and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'editworkflowevent', 
                    Description: '@ConEdiAB@',
                    EventType : 'special', 
                    Topic : 'Configuration',
                    TableName: 'Configs',
                    ValidationRules: [
                    ],
                }";
    }
}
