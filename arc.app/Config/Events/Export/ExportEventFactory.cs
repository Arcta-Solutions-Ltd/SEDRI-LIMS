using arc.app.Common;
using arc.app.Config.Events.Export;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Factory that resolves an export-area event configuration by its lower-case name.
    /// New export events (including <c>saveexportprofilemapping</c>) are registered here.
    /// </summary>
    internal class ExportEventFactory : IDefinitionFactory
    {
        /// <summary>
        /// Returns the <see cref="IDefinition"/> for an export event, or <c>null</c> when the name is unknown.
        /// </summary>
        /// <param name="definitionName">The event configuration name (case-insensitive).</param>
        /// <returns>The matching configuration definition, or <c>null</c> if no match is found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "whonetexport" => new WHONETExportEventConfig(),
                "dhis2export" => new DHIS2ExportEventConfig(),
                "addexportprofile" => new AddExportProfileEventConfig(),
                "addexportprofilefield" => new AddExportProfileFieldEventConfig(),
                "editexportprofile" => new EditExportProfileEventConfig(),
                "editexportprofilefields" => new EditExportProfileFieldsConfig(),
                "deleteexportprofile" => new DeleteExportProfileEventConfig(),
                "deleteexportprofilefield" => new DeleteExportProfileFieldConfig(),
                "runexportprofile" => new RunExportProfileEventConfig(),
                "addexportschedule" => new AddExportScheduleEventConfig(),
                "editexportschedule" => new EditExportScheduleEventConfig(),
                "deleteexportschedule" => new DeleteExportScheduleEventConfig(),
                "saveexportprofilemapping" => new SaveExportProfileMappingEventConfig(),
                _ => null,
            };
        }
    }
}
