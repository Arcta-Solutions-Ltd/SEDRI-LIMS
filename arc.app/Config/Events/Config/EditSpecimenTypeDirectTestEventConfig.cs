using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Specimen Type Direct Test" event.
/// </summary>
internal class EditSpecimenTypeDirectTestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Direct Test" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the attributes for the "Edit Specimen Type Direct Test" event,
    /// including its name, description, event type, and topic. It also specifies validation rules 
    /// to ensure required fields, such as "GroupId" and "AssociatedListId," are populated, 
    /// providing appropriate error messages when these fields are not satisfied.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, event type, topic, and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'editSpecimenTypeDirectTest',
                        Description: '@ConEdiE@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@BreA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAA@'}
                        ]
                    }";
    }
}
