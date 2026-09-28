using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Culture Type Culture Test" event.
/// </summary>
internal class AddCultureTypeCultureTestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Culture Test" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, associated table name, topic, and validation rules.
    /// It ensures that required fields, such as "GroupId" and "AssociatedListId," are validated before 
    /// executing the event within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, validation rules, table name, and topic.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'addCultureTypeCultureTest',
                        Description: '@ConAddZ@',
                        EventType : 'specialadddata',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@ConA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}
