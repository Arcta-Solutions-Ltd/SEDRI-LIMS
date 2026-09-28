using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page for adding/editing a Culture Test TAT override (TestName + range).
/// </summary>
internal class AddCultureTestOverridePageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'addculturetestoverridepage',
                            pageTitle: '@GenTATI@',
                            text: '@GenTATC@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TestName', type: 'combobox', label: '@GenTesD@', optionsName: 'culturetestconfiglist', required: true },
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
