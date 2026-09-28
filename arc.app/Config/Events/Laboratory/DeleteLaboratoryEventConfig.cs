using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Laboratory" event.
/// </summary>
internal class DeleteLaboratoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Laboratory" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event name, description, type, topic, and table name associated 
    /// with deleting laboratory records. It also specifies data rules for validations, including 
    /// checks for user counts, specimen counts, and the existence of at least one record.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including its metadata and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteLaboratory',
                        Description: '@LabDel@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        TableName: 'Laboratory',
                        DataRules: [
                            { type: 'NoRecord', query: 'laboratoryusercount', message: '@LabThi@' },
                            { type: 'NoRecord', query: 'laboratoryspecimencount', message: '@LabThiA@' },
                            { type:'OneRecord', query: 'laboratorycount', message:'@LabThiB@' }
                        ]
                    }";
    }
}
