using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the Add/Edit Turn Around Time range page.
/// </summary>
internal class AddTurnAroundTimeRangePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'addturnaroundtimerangepage',
                            pageTitle: '@GenTATB@',
                            text: '@GenTATC@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
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
