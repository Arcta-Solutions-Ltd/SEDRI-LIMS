using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration for the Neoshield neonatal specimen request raised from the patient list, where the
/// baby is already selected so the search and patient registration pages are not needed.
/// </summary>
internal class CreateNeoshieldSpecimenForPatientFormConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string representing the patient scoped Neoshield request form configuration.
    /// </summary>
    /// <returns>A JSON-formatted string containing the form metadata, page sequence, page visibility
    /// rules and the repeat definition for adding further specimens.</returns>
    public string Get()
    {
        var form =
            @"{
                name: 'createneoshieldspecimenforpatientform',
                title: '@NeoNeo@',
                singleItemName: 'specimen',
                initialQuery: 'blankculturetypeandtestselectionwithpatientref',
                startstate: 'notrejected',
                saveevent: 'neoshieldspecimen',
                recordView: 'specimenrecordview',
                suppressRecordView: true,
                configurable: 'Yes',
                defaultView: 'specimens',
                pages: [
                    'admissionselectionpage',
                    'neoshieldadmissionpage',
                    'requestselectionforadmissionpage',
                    'neoshieldrequestheaderpage',
                    'neoshieldclinicalstatepage',
                    'neoshieldspecimenpage',
                    'neoshieldbottlepage',
                    'testselectionpage'
                ],
                rules: [
                    { Outcome: 'visible', page: 'neoshieldadmissionpage', state: 'newadmission' },
                    { Outcome: 'visible', page: 'neoshieldrequestheaderpage', state: 'newrequest' },
                    { Outcome: 'visible', page: 'neoshieldclinicalstatepage', state: 'newrequest' },
                    { Outcome: 'visible', page: 'neoshieldbottlepage', state: 'bloodspecimen' }
                ],
                repeat: { fromPage: 'neoshieldspecimenpage', prompt: '@NeoAddAnoA@', title: '@NeoAddAno@' }
            }";

        return form;
    }
}
