using arc.app.Common;

namespace arc.app.Config.Forms.Export;

/// <summary>
/// Configuration for the "Edit Export Schedule" form.
/// Two-page form: criteria then schedule.
/// </summary>
internal class EditExportScheduleFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Edit Export Schedule form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'editexportscheduleform',
            viewTitle: '@ExpSchEdit@',
            saveEvent: 'editexportschedule',
            initialQuery: 'editexportschedule',
            suppressRecordView: true,
            pages: ['exportschedulecriteriapage', 'exportscheduleschedulepage']
        }";

        return form;
    }
}
