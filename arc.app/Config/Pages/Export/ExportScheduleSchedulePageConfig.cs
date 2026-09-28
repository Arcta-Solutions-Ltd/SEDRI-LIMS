using arc.app.Common;

namespace arc.app.Config.Pages.Export;

/// <summary>
/// Page 2 of the export schedule form: frequency, time of day, day of month, incremental.
/// </summary>
internal class ExportScheduleSchedulePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the export schedule timing page.
    /// </summary>
    /// <remarks>
    /// The <c>OutputDirectory</c> placeholder contains the <c>{STORAGE_ROOT}</c> token, which is
    /// substituted with the configured <c>Files:StorageRoot</c> value (forward-slashed) when the
    /// configuration is assembled in <see cref="arc.app.Config.HandleConfig"/>.
    /// </remarks>
    public string Get()
    {
        var page = @"{
            name: 'exportscheduleschedulepage',
            pageTitle: '@ExpSchTim@',
            text: '@ExpSchTimD@',
            required: 'Frequency',
            requiredRule: 'and',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg0',
                            fields: [
                                { id: 'OutputDirectory', type: 'singleline', label: '@ExpSchOutDir@', placeholder: 'e.g. {STORAGE_ROOT}/myschedule' }
                            ]
                        },
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'Frequency', type: 'combobox', label: '@ExpSchFreq@', required: true, optionsName: 'ExportScheduleFrequency', defaultValue: '1527' },
                                { id: 'ChangesToInclude', type: 'radio', label: '@ExpSchChanges@', optionsName: 'ExportScheduleChangesToInclude', defaultValue: '1530' },
                                { id: 'Enabled', type: 'toggle', label: '@ExpSchEn@', defaultValue: 'Yes' }
                            ]
                        },
                        {
                            key: 'fg2',
                            rules: [{ effect: 'visible', field: 'Frequency', rule: '=', value: '1527, 1528' }],
                            fields: [
                                { id: 'TimeOfDay', type: 'time', label: '@ExpSchTime24@' }
                            ]
                        },
                        {
                            key: 'fg3',
                            rules: [{ effect: 'visible', field: 'Frequency', rule: '=', value: '1528' }],
                            fields: [
                                { id: 'DayOfMonth', type: 'number', label: '@ExpSchDay@', min: 1, max: 31 }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}
