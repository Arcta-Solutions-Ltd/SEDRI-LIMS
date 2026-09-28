using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Defines the configuration for the "addLaboratory" event.
/// </summary>
internal class AddLaboratoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event and validation rules.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'addLaboratory', 
                    Description: '@LabAdd@',
                    EventType : 'adddata', 
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
