using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Specimen Type Culture Type Option" event.
/// </summary>
internal class AddSpecimenTypeCultureTypeOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Type Culture Type Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, topic, and validation rules.
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
                        EventName: 'addSpecimenTypeCultureTypeOptionEvent',
                        Description: '@LabAddH@',
                        EventType : 'specialadddata',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@BreA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
    }
}
