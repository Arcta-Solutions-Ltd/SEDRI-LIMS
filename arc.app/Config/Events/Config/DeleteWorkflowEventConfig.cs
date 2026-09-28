using arc.app.Common;

namespace arc.app.Config.Events.Config;

/// <summary>
/// Configuration for the "Delete Workflow" event.
/// </summary>
internal class DeleteWorkflowEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Workflow" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the attributes for the "Delete Workflow" event, including:
    /// - EventName: Specifies the name of the event.
    /// - Description: Provides a brief description of the event's purpose.
    /// - EventType: Indicates the type of event, categorized as "deletedata."
    /// - Topic: Identifies the event's context, which is "Configuration."
    /// - TableName: Specifies the associated database table, "Configs."
    /// While validation rules are included as an array, none are specified in this implementation.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, event type, topic, table name, and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deleteworkflowevent', 
                    Description: '@ConDelAC@',
                    EventType : 'deletedata', 
                    Topic : 'Configuration',
                    TableName: 'Configs',
                    ValidationRules: [
                    ],
                }";
    }
}
