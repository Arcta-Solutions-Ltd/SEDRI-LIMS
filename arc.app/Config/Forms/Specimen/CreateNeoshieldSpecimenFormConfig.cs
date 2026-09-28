using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration for the Neoshield neonatal specimen request raised from the specimen list, where the
/// baby still has to be found or registered. The page visibility rules follow the states raised by the
/// search and selection pages, so the patient, admission and request data entry pages only appear when the
/// user chose to create a new one of each.
/// </summary>
internal class CreateNeoshieldSpecimenFormConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string representing the Neoshield request form configuration.
    /// </summary>
    /// <returns>A JSON-formatted string containing the form metadata, page sequence, page visibility
    /// rules and the repeat definition for adding further specimens.</returns>
    public string Get()
    {
        var form =
            @"{
                name: 'createneoshieldspecimenform',
                title: '@NeoNeo@',
                singleItemName: 'specimen',
                initialQuery: 'blankculturetypeandtestselection',
                startstate: 'notrejected',
                saveevent: 'neoshieldspecimen',
                recordView: 'specimenrecordview',
                suppressRecordView: true,
                configurable: 'Yes',
                defaultView: 'specimens',
                pages: [
                    'patientsearchpage',
                    'patientsearchresultspage',
                    'neoshieldpatientidentificationpage',
                    'neoshieldbirthdetailspage',
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
                    { Outcome: 'visible', page: 'neoshieldpatientidentificationpage', state: 'newpatient' },
                    { Outcome: 'visible', page: 'neoshieldbirthdetailspage', state: 'newpatient' },
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
