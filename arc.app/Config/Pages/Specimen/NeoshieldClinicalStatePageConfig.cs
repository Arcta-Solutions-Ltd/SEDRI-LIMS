using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the clinical state step of the Neoshield neonatal request
/// (specification fields 31 to 43). Only shown when the user chose to create a new request, since the
/// values are a snapshot of the baby at the moment the request was raised.
/// </summary>
internal class NeoshieldClinicalStatePageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield clinical state page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldclinicalstatepage',
                        pageTitle: '@NeoCliSta@',
                        text: '@NeoCliStaA@.',
                        required: 'WeightKnownId,AntibioticReceivedId,RecentSurgeryId,CentralLineInPlaceId,RespiratorySupportId',
                        requiredRule: 'and',
                        pageGroup: 'request',
                        tablename: 'request',
                        columns: [
                            {
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'WeightKnownId', type: 'combobox', label: '@NeoWeiKno@', required: true, multiselect: true, placeholder: '@GenSelK@', optionsName: 'NeoWeightKnown' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'WeightKnownId', rule: 'contains', value: '2103' }],
                                        fields: [
                                            { id: 'CurrentWeight', type: 'number', label: '@NeoCurWei@', min: 300, max: 8000 }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        rules: [{ effect: 'visible', field: 'WeightKnownId', rule: 'contains', value: '2104' }],
                                        fields: [
                                            { id: 'CurrentWeightDate', type: 'date', label: '@NeoCurWeiDat@', placeholder: '@PatSelF@', Min: 'now y-2', Max: 'now' }
                                        ]
                                    },
                                    {
                                        key: 'fg4',
                                        fields: [
                                            { id: 'AntibioticReceivedId', type: 'radio', label: '@NeoAntRec@', required: true, optionsName: 'NeoYesNoUnknown' }
                                        ]
                                    },
                                    {
                                        key: 'fg5',
                                        rules: [{ effect: 'visible', field: 'AntibioticReceivedId', rule: '=', value: '1942' }],
                                        fields: [
                                            { id: 'AntibioticTimingId', type: 'radio', label: '@NeoAntTim@', optionsName: 'NeoAntibioticTiming' },
                                            { id: 'AntibioticAgentsId', type: 'combobox', label: '@NeoAntAge@', multiselect: true, placeholder: '@GenSelK@', optionsName: 'NeoAntibioticAgent' },
                                            { id: 'AntibioticStartKnownId', type: 'combobox', label: '@NeoAntStaKno@', multiselect: true, placeholder: '@GenSelK@', optionsName: 'NeoDateTimeKnown' }
                                        ]
                                    },
                                    {
                                        key: 'fg6',
                                        rules: [{ effect: 'visible', field: 'AntibioticStartKnownId', rule: 'contains', value: '1947' }],
                                        fields: [
                                            { id: 'AntibioticStartDate', type: 'date', label: '@NeoAntStaDat@', placeholder: '@PatSelF@', Min: 'now y-2', Max: 'now' }
                                        ]
                                    },
                                    {
                                        key: 'fg7',
                                        rules: [{ effect: 'visible', field: 'AntibioticStartKnownId', rule: 'contains', value: '1948' }],
                                        fields: [
                                            { id: 'AntibioticStartTime', type: 'time', label: '@NeoAntStaTim@', placeholder: '@SpeEntI@', mask: '99:99' }
                                        ]
                                    },
                                    {
                                        key: 'fg8',
                                        fields: [
                                            { id: 'RecentSurgeryId', type: 'radio', label: '@NeoRecSur@', required: true, optionsName: 'NeoRecentSurgery' },
                                            { id: 'CentralLineInPlaceId', type: 'radio', label: '@NeoCenLin@', required: true, optionsName: 'NeoYesNoUnknown' }
                                        ]
                                    },
                                    {
                                        key: 'fg9',
                                        rules: [{ effect: 'visible', field: 'CentralLineInPlaceId', rule: '=', value: '1942' }],
                                        fields: [
                                            { id: 'CentralLineTypeId', type: 'combobox', label: '@NeoCenLinTyp@', placeholder: '@GenSelK@', optionsName: 'NeoCentralLineType' }
                                        ]
                                    },
                                    {
                                        key: 'fg10',
                                        fields: [
                                            { id: 'RespiratorySupportId', type: 'radio', label: '@NeoResSup@', required: true, optionsName: 'NeoRespiratorySupport' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
