using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Culture Type Category" event.
/// </summary>
internal class EditCultureTypeCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, table name, and topic.
    /// It includes validation rules to ensure required fields, such as "AssociatedListId," are properly specified.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, and associated table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'EditCultureTypeCategoryEvent',
                        Description: '@LabEdiF@',
                        EventType : 'special',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
    }
}
