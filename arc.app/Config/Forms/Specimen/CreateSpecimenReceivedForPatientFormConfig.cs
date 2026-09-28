using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the JSON form configuration for creating a received specimen form for a patient.
/// Implements <see cref="IDefinition"/> to supply the required form metadata and rules.
/// </summary>
internal class CreateSpecimenReceivedForPatientFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that describes the patient specimen received form configuration,
    /// including form metadata, page sequence, and visibility rules.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing:
    /// - Form name, title, and item naming
    /// - Initial query and save event identifiers
    /// - Record view settings and configurability flags
    /// - Ordered pages and conditional visibility rules
    /// </returns>
    public string Get()
    {
        var form =
            @"{
                    name: 'createspecimenreceivedforpatientform',
                    title: '@SpeAddE@',
                    singleItemName: 'specimen',
                    initialQuery: 'blankculturetypeandtestselectionwithpatientref',
                    startstate: 'notrejected',
                    saveevent: 'newreceivedspecimen',
                    recordView: 'specimenrecordview',
                    suppressRecordView: true,
                    configurable: 'Yes',
                    defaultView: 'specimens',
                    pages: ['advancespecimendetailspage', 'specimenattributes', 'specimentimingsreceived', 'ackreceiptpage', 'testselectionpage', 'culturetypeselectionpage'],
                    rules:
                    [
                        { Outcome: 'visible', page: 'testselectionpage', state: 'notrejected' },
                        { Outcome: 'visible', page: 'culturetypeselectionpage', state: 'notrejected' }
                    ],
                }";

        return form;
    }
}


