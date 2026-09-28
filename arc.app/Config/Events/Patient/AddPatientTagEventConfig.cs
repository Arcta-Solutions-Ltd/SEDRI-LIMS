using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Patient Tag" event.
/// </summary>
internal class AddPatientTagEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        EventName: 'addpatienttag',
                        Description: '@GenTagG@',
                        EventType: 'specialadddata',
                        Topic: 'Tags',
                        TableName: 'PatientTag',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenReqB@' }
                        ]
                    }";
    }
}
