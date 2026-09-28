using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the specimen step of the Neoshield neonatal request
/// (specification fields 45 to 53; field 44 is the accession number stamped by the server and field 46 is
/// collected on the test selection page so it inherits the existing specimen type filtering).
/// </summary>
internal class NeoshieldSpecimenPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield specimen page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldspecimenpage',
                        pageTitle: '@NeoSpe@',
                        text: '@NeoSpeA@.',
                        required: 'SpecimenTypeId,CollectionDate,CollectedById,SpecimenLabelledId',
                        requiredRule: 'and',
                        nextButton: { onclickstate: { state: 'bloodspecimen',
                                                      configurable: 'Yes',
                                                      rules: [{ effect: 'bloodspecimen', field: 'SpecimenTypeId', rule: '=', value: '808' }]
                                                    },
                                      buttontext: '@GenNex@',
                                      show: true
                                    },
                        columns: [
                            {
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'SpecimenTypeId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType' },
                                            { id: 'CollectionDate', type: 'date', label: '@SpeColC@', required: true, defaultToNow: true, placeholder: '@SpeSelI@', Min: 'now d-300', Max: 'now' },
                                            { id: 'CollectionTime', type: 'time', label: '@SpeColD@', defaultToNow: true, placeholder: '@SpeEntI@', mask: '99:99' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'SpecimenTypeId', rule: '!=', value: '808' }],
                                        fields: [
                                            { id: 'SpecimenSiteId', type: 'combobox', label: '@SpeSpeC@', multiselect: false, placeholder: '@SpeSelD@', optionsName: 'SpecimenSite', parentList: 'SpecimenTypeId' },
                                            { id: 'BodySideId', type: 'radio', label: '@NeoBodSid@', optionsName: 'NeoBodySide' }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        rules: [{ effect: 'visible', field: 'SpecimenTypeId', rule: '=', value: '808' }],
                                        fields: [
                                            { id: 'CollectionMethodId', type: 'combobox', label: '@NeoColMet@', placeholder: '@GenSelK@', optionsName: 'NeoCollectionMethod' }
                                        ]
                                    },
                                    {
                                        key: 'fg4',
                                        fields: [
                                            { id: 'CollectedById', type: 'combobox', label: '@NeoColBy@', required: true, placeholder: '@GenSelK@', optionsName: 'NeoCollectedBy' },
                                            { id: 'SpecimenLabelledId', type: 'radio', label: '@NeoSpeLab@', required: true, optionsName: 'NeoYesNoUnknown' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
