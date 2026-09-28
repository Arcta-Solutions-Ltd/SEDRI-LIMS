using arc.app.Config.Views.RecordViews;
using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Factory class for creating record view configurations.
/// </summary>
public class RecordViewConfigFactory : IRecordViewConfigFactory
{
    /// <summary>
    /// Retrieves a specific record view configuration based on the provided view name.
    /// </summary>
    /// <param name="viewName">The name of the record view configuration to retrieve.</param>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object representing the requested view configuration, 
    /// or <c>null</c> if the view name does not match any known configurations.
    /// </returns>
    public async Task<RecordViewConfig> GetViewAsync(string viewName)
    {
        return viewName.ToLower() switch
        {
            "alerts" => new AlertRecordViewConfig().GetView(),
            "admissionrecordview" => new AdmissionRecordViewConfig().GetView(),
            "breakpoints" => new BreakpointRecordViewConfig().GetView(),
            "expertrules" => new ExpertRuleRecordViewConfig().GetView(),
            "cultures" => new CultureRecordViewConfig().GetView(),
            "exportprofile" => new ExportProfileRecordViewConfig().GetView(),
            "exporthistory" => new ExportHistoryRecordViewConfig().GetView(),
            "formconfig" => new FormConfigViewConfig().GetView(),
            "importprofile" => new ImportProfileViewConfig().GetView(),
            "laboratories" => new LaboratoryRecordViewConfig().GetView(),
            "iqctestprofile" => new IqcTestProfileRecordView().GetView(),
            "iqctests" => new IqcTestRecordViewConfig().GetView(),
            "patientrecordview" => new PatientRecordViewConfig().GetView(),
            "reportlistconfig" => new ReportListConfigViewConfig().GetView(),
            "requestrecordview" => new RequestRecordViewConfig().GetView(),
            "roles" => new RoleRecordViewConfig().GetView(),
            "settings" => new SettingsRecordViewConfig().GetView(),
            "specimens" => new SpecimenRecordViewConfig().GetView(),
            "testrecordview" => new TestRecordViewConfig().GetView(),
            "instrumentresultrecordview" => new InstrumentResultRecordViewConfig().GetView(),
            "specimenreportview" => new SpecimenReportViewConfig().GetView(),
            "views" => new ConfigRecordViewConfig().GetView(),
            "workflows" => new WorkflowViewConfig().GetView(),
            _ => null
        };
    }
}
