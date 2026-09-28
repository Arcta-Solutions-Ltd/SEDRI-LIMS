using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the request header step of the Neoshield neonatal request
/// (specification fields 26 to 30; fields 18 to 25 are stamped by the server). Only shown when the user
/// chose to create a new request.
/// </summary>
internal class NeoshieldRequestHeaderPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield request header page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldrequestheaderpage',
                        pageTitle: '@NeoReqHea@',
                        text: '@NeoReqA@.',
                        required: 'WardId,CotAvailableId,UrgencyId,IndicationId',
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
                                            { id: 'WardId', type: 'combobox', label: '@NeoWar@', required: true, placeholder: '@GenSelK@', optionsName: 'NeoWard' },
                                            { id: 'CotAvailableId', type: 'radio', label: '@NeoCotAva@', required: true, optionsName: 'NeoYesNo' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'CotAvailableId', rule: '=', value: '1940' }],
                                        fields: [
                                            { id: 'CotId', type: 'combobox', label: '@NeoCot@', placeholder: '@GenSelK@', optionsName: 'NeoCotIdentifier' }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        fields: [
                                            { id: 'UrgencyId', type: 'radio', label: '@NeoUrg@', required: true, optionsName: 'NeoUrgency' },
                                            { id: 'IndicationId', type: 'combobox', label: '@NeoInd@', required: true, placeholder: '@GenSelD@', optionsName: 'NeoIndication' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
