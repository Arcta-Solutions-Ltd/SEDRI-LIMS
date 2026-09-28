using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Specimen Tag" event.
/// </summary>
internal class AddSpecimenTagEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        EventName: 'addspecimentag',
                        Description: '@GenTagF@',
                        EventType: 'specialadddata',
                        Topic: 'Tags',
                        TableName: 'SpecimenTag',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenReqB@' }
                        ]
                    }";
    }
}
