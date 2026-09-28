using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page for adding/editing a Direct Test TAT override (TestName + range).
/// </summary>
internal class AddDirectTestOverridePageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'adddirecttestoverridepage',
                            pageTitle: '@GenTATG@',
                            text: '@GenTATC@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TestName', type: 'combobox', label: '@GenTesD@', optionsName: 'directtestconfiglist', required: true },
                                                { id: 'RangeFrom', type: 'duration', label: '@GenTATL@' },
                                                { id: 'RangeTo', type: 'duration', label: '@GenTATM@' },
                                                { id: 'Colour', type: 'colourpicker', label: '@GenColA@', required: true }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
