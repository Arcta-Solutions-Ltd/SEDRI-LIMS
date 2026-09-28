using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the JSON form configuration for creating a specimen received form.
/// Implements <see cref="IDefinition"/> to supply the necessary form metadata,
/// page sequence, and visibility rules for the specimen reception workflow.
/// </summary>
internal class CreateSpecimenReceivedFormConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string representing the received specimen form configuration.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing:
    /// - Form metadata (name, title, singleItemName, initialQuery, startstate, saveevent, etc.)
    /// - Record view settings and configurability flags
    /// - Ordered pages for patient and specimen data entry
    /// - Visibility rules for conditional page display
    /// </returns>
    public string Get()
    {
        var form =
            @"{
                name: 'createspecimenreceivedform',
                title: '@SpeAddE@',
                singleItemName: 'specimen',
                initialQuery: 'blankculturetypeandtestselection',
                startstate: 'notrejected',
                saveevent: 'newreceivedspecimen',

                    recordView: 'specimenrecordview',

                suppressRecordView: true,
                configurable: 'Yes',
                pages: [
                    'patientsearchpage',
                    'patientsearchresultspage',
                    'patientdetailspage',
                    'patientaddresspage',
                    'advancespecimendetailspage',
                    'specimenattributes',
                    'specimentimingsreceived',
                    'ackreceiptpage',
                    'testselectionpage',
                    'culturetypeselectionpage'
                ],
                rules: [
                    { Outcome: 'visible', page: 'patientdetailspage', state: 'newpatient' },
                    { Outcome: 'visible', page: 'patientaddresspage', state: 'newpatient' },
                    { Outcome: 'visible', page: 'testselectionpage', state: 'notrejected' },
                    { Outcome: 'visible', page: 'culturetypeselectionpage', state: 'notrejected' }
                ]
            }";

        return form;
    }
}


