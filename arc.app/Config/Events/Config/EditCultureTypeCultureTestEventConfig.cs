using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Culture Type Culture Test" event.
/// </summary>
internal class EditCultureTypeCultureTestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Culture Test" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, associated table name, topic, and validation rules.
    /// It ensures that required fields, such as "GroupId" and "AssociatedListId," are validated before
    /// executing the event within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, and associated table name and topic.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'editCultureTypeCultureTest',
                        Description: '@ConEdiAA@',
                        EventType : 'special',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}
