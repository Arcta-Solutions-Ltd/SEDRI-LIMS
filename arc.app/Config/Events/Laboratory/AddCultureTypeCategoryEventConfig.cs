using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Culture Type Category" event.
/// </summary>
internal class AddCultureTypeCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, table name, topic, 
    /// and validation rules. It ensures that required fields, such as "GroupId" and "AssociatedListId," 
    /// are properly validated before executing the event.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, and associated table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'AddCultureTypeCategoryEvent',
                        Description: '@LabAddD@',
                        EventType : 'specialadddata',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@LabAC@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
    }
}
