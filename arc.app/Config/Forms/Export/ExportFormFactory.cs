using arc.app.Common;
using arc.app.Config.Forms.Export;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Factory that resolves an export-area form configuration by its lower-case name.
    /// New export forms (including <c>manageexportprofilemappingform</c>) are registered here.
    /// </summary>
    internal class ExportFormFactory : IDefinitionFactory
    {
        /// <summary>
        /// Returns the <see cref="IDefinition"/> for an export form, or <c>null</c> when the name is unknown.
        /// </summary>
        /// <param name="definitionName">The form configuration name (case-insensitive).</param>
        /// <returns>The matching configuration definition, or <c>null</c> if no match is found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "whonetexportform" => new WHONETExportFormConfig(),
                "dhis2exportform" => new DHIS2ExportFormConfig(),
                "addexportprofileform" => new AddExportProfileFormConfig(),
                "addexportprofilefieldform" => new AddExportProfileFieldFormConfig(),
                "editexportprofileform" => new EditExportProfileFormConfig(),
                "editexportprofilefieldsform" => new EditExportProfileFieldsFormConfig(),
                "deleteexportprofileform" => new DeleteExportProfileFormConfig(),
                "deleteexportprofilefieldform" => new DeleteExportProfileFieldFormConfig(),
                "runexportform" => new RunExportFormConfig(),
                "addexportscheduleform" => new AddExportScheduleFormConfig(),
                "editexportscheduleform" => new EditExportScheduleFormConfig(),
                "deleteexportscheduleform" => new DeleteExportScheduleFormConfig(),
                "manageexportprofilemappingform" => new ManageExportProfileMappingFormConfig(),
                _ => null,
            };
        }
    }
}
