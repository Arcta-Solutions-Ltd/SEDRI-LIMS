using arc.app.Common;

namespace arc.app.Config.Forms.Export;

/// <summary>
/// Configuration for the "Add Export Schedule" form.
/// Two-page form: criteria then schedule.
/// </summary>
internal class AddExportScheduleFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Add Export Schedule form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'addexportscheduleform',
            viewTitle: '@ExpSchAdd@',
            saveEvent: 'addexportschedule',
            suppressRecordView: true,
            InitialQuery: 'exportscheduleforminitialquery',
            pages: ['exportschedulecriteriapage', 'exportscheduleschedulepage']
        }";

        return form;
    }
}
