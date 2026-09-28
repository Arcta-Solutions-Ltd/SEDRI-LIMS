using arc.app.Common;

namespace arc.app.Config.Pages.Export;

/// <summary>
/// Page for the delete export schedule confirmation form.
/// </summary>
internal class DeleteExportSchedulePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the delete export schedule page.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'deleteexportschedulepage',
            pageTitle: '@ExpSchDel@',
            text: '@ExpSchDelD@',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'Name', type: 'text', label: '@GenNam@', required: true }
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenDelC@' }
        }";

        return page;
    }
}
