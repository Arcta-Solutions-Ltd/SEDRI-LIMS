using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration for the Specimen Timings page used in the received specimen workflows.
/// Collection date and time default to the current date/time via <c>defaultToNow</c>, matching
/// <see cref="SpecimenTimingsPageConfig"/>; users may change them when collection occurred earlier.
/// </summary>
internal class SpecimenTimingsForReceivedPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the received-specimen timing page configuration.
    /// </summary>
    /// <returns>
    /// A JSON string defining the layout, required fields, and behaviour for specimen timing entry
    /// in the received-specimen workflows. <c>CollectionDate</c> and <c>CollectionTime</c> use
    /// <c>defaultToNow</c> so the portal pre-fills today's date and the current time.
    /// </returns>
    public string Get()
    {
        var page = @"{
            name: 'specimentimingsreceived',
            pageTitle: '@CusSpe@',
            text: '@CusEntC@.',
            required: 'SpecimenTypeId,CollectionDate',
            configureActions: 'add,edit,delete',
            requiredRule: 'and',
            tablename: 'specimen',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'SpecimenTypeId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType', tab: true, Configurable: 'No' },
                                { id: 'SpecimenSiteId', type: 'combobox', label: '@SpeSpeC@', multiselect: false, placeholder: '@SpeSelD@', optionsName: 'SpecimenSite', parentList: 'SpecimenTypeId', tab: true, Configurable: 'No' },
                                { id: 'CollectionDate', type: 'date', label: '@SpeColC@', required: true, defaultToNow: true, placeholder: '@SpeSelI@', Min: 'now d-300', Max: 'now' },
                                { id: 'CollectionTime', type: 'time', label: '@SpeColD@', defaultToNow: true, placeholder: '@SpeEntI@', mask: '99:99' },
                                { id: 'Age', type: 'age', label: '@SpeAge@', required: false, Configurable: 'No' },
                                { id: 'ExistingBarCode', type: 'singleline', label: '@SpeExi@', required: false, Max: 30 },
                                { id: 'FurtherInformation', type: 'multiline', label: '@CusFur@', required: false, placeholder: '@CusEntB@' }
                                //{ id: 'RequestCultureTests', type: 'toggle', label: '@CusReq@', required: false },
                                //{ id: 'MelioidosisCultureTests', type: 'toggle', label: '@CusMel@', required: false }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}
