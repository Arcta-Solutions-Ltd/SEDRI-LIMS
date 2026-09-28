using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Culture Type Culture Test Option" event.
/// </summary>
internal class EditCultureTypeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Culture Test Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, topic, and validation rules.
    /// It ensures that required fields, such as "GroupId" and "AssociatedListId," are properly validated
    /// before executing the event within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, and associated topic.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'editCultureTypeCultureTestOptionEvent',
                        Description: '@LabEdiK@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}
