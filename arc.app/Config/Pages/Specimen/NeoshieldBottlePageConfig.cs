using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the blood culture bottle step of the Neoshield neonatal request
/// (specification fields 54 to 57). The whole page is only reached for a blood specimen, which the
/// specimen page signals with the <c>bloodspecimen</c> form state.
/// </summary>
internal class NeoshieldBottlePageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield blood culture bottle page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldbottlepage',
                        pageTitle: '@NeoBot@',
                        text: '@NeoBotA@.',
                        required: 'VolumeMethodId,BottleTypeId',
                        requiredRule: 'and',
                        columns: [
                            {
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'VolumeMethodId', type: 'radio', label: '@NeoVolMet@', required: true, optionsName: 'NeoVolumeMethod' },
                                            { id: 'BottleTypeId', type: 'combobox', label: '@NeoBotTyp@', required: true, placeholder: '@GenSelK@', optionsName: 'NeoBottleType' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'VolumeMethodId', rule: '=', value: '2168' }],
                                        fields: [
                                            { id: 'BottleOnlyWeight', type: 'number', label: '@NeoBotWei@', decimals: 1, min: 0, max: 200 }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        rules: [{ effect: 'visible', field: 'VolumeMethodId', rule: '=', value: '2169' }],
                                        fields: [
                                            { id: 'EstimatedBloodVolume', type: 'number', label: '@NeoEstVol@', decimals: 1, min: 0, max: 20 }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
