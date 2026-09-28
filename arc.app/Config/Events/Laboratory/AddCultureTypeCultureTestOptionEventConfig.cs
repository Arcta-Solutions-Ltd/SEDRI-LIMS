using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Culture Type Culture Test Option" event.
/// </summary>
internal class AddCultureTypeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Culture Test Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, topic, and validation rules.
    /// It ensures that required fields, such as "GroupId" and "AssociatedListId," are validated before 
    /// executing the event within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, and associated topic.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'addCultureTypeCultureTestOptionEvent',
                        Description: '@LabAddI@',
                        EventType : 'specialadddata',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@ConA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}

