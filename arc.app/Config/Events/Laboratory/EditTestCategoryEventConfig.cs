using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Test Category" event.
/// </summary>
internal class EditTestCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Test Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, table name, and associated topic.
    /// It includes validation rules to ensure required fields, such as "AssociatedListId," are properly specified.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, type, topic, and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'EditTestCategoryEvent',
                        Description: '@LabEdiC@',
                        EventType : 'special',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAA@'}
                        ]
                    }";
    }
}
