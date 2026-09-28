using arc.app.Common;

namespace arc.app.Config.Forms.Export;

/// <summary>
/// Configuration for the "Delete Export Schedule" form.
/// </summary>
internal class DeleteExportScheduleFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Delete Export Schedule form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'deleteexportscheduleform',
            viewTitle: '@ExpSchDel@',
            saveEvent: 'deleteexportschedule',
            initialQuery: 'editexportschedule',
            suppressRecordView: true,
            pages: ['deleteexportschedulepage']
        }";

        return form;
    }
}
