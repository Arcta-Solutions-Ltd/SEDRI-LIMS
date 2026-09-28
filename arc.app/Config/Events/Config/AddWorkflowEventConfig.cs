using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Workflow" event.
/// </summary>
internal class AddWorkflowEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies attributes such as the event name, description, event type, 
    /// topic, and table name. It is designed to define the structure of the event for adding workflows 
    /// within the configuration context. Although there are no validation rules specified here, 
    /// the configuration can accommodate them if required.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as the event name, 
    /// description, type, topic, and associated table name.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'addworkflowevent', 
                        Description: '@ConAddAB@',
                        EventType : 'specialadddata', 
                        Topic : 'Configuration',
                        TableName: 'Configs',
                        ValidationRules: [
                        ],
                    }";
    }
}
