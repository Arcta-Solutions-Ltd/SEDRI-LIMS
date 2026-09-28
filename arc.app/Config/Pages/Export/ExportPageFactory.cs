using arc.app.Common;
using arc.app.Config.Pages.Export;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Factory that resolves an export-area page configuration by its lower-case name.
    /// New export pages (including <c>manageexportprofilemappingpage</c>) are registered here.
    /// </summary>
    internal class ExportPageFactory : IDefinitionFactory
    {
        /// <summary>
        /// Returns the <see cref="IDefinition"/> for an export page, or <c>null</c> when the name is unknown.
        /// </summary>
        /// <param name="definitionName">The page configuration name (case-insensitive).</param>
        /// <returns>The matching configuration definition, or <c>null</c> if no match is found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "whonetexportpage" => new WHONETExportPageConfig(),
                "dhis2exportpage" => new DHIS2ExportPageConfig(),
                "addexportprofilepage" => new AddExportProfilePageConfig(),
                "addexportprofilefieldpage" => new AddExportProfileFieldPageConfig(),
                "editexportprofilepage" => new EditExportProfilePageConfig(),
                "editexportprofilefieldspage" => new EditExportProfileFieldsPageConfig(),
                "deleteexportprofilepage"=> new DeleteExportProfilePageConfig(),
                "deleteexportprofilefieldpage" => new DeleteExportProfileFieldPageConfig(),
                "runexportpage" => new RunExportPageConfig(),
                "exportschedulecriteriapage" => new ExportScheduleCriteriaPageConfig(),
                "exportscheduleschedulepage" => new ExportScheduleSchedulePageConfig(),
                "deleteexportschedulepage" => new DeleteExportSchedulePageConfig(),
                "manageexportprofilemappingpage" => new ManageExportProfileMappingPageConfig(),
                _ => null,
            };
        }
    }
}
