using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Defines the configuration for the "editLaboratory" event.
/// </summary>
internal class EditLaboratoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event type, topic, table name, and validation rules.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'editLaboratory', 
                    Description: '@LabEdi@',
                    EventType : 'editdata', 
                    Topic : 'Laboratory', 
                    TableName: 'Laboratory',
                    ValidationRules: [
                        { field: 'LaboratoryName', rule: 'required', message: '@LabLabB@'},
                        { field: 'LanguageId', rule: 'required', message: '@LabA@'},
                        { field: 'DefaultWorkflowId', rule: 'required', message: '@LabAE@'}
                    ]
                }";
    }
}
