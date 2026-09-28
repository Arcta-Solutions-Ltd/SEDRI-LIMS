using arc.app.Common;
using arc.app.Config.UIEvents.Export;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Factory that resolves an export-area UI event configuration by its lower-case name.
    /// New export UI events (including <c>manageexportprofilemappinguievent</c>) are registered here.
    /// </summary>
    internal class ExportUIEventFactory : IDefinitionFactory
    {
        /// <summary>
        /// Returns the <see cref="IDefinition"/> for an export UI event, or <c>null</c> when the name is unknown.
        /// </summary>
        /// <param name="definitionName">The UI event configuration name (case-insensitive).</param>
        /// <returns>The matching configuration definition, or <c>null</c> if no match is found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "whonetexport" => new WHONETExportUIEventConfig(),
                "dhis2export" => new DHIS2ExportUIEventConfig(),
                "addexportprofileuievent" => new AddExportProfileUIEventConfig(),
                "addexportprofilefielduievent" => new AddExportProfileFieldUIEventConfig(),
                "editexportprofileuievent" => new EditExportProfileUIEventConfig(),
                "editexportprofilefieldsuievent" => new EditExportProfileFieldsUIEventConfig(),
                "deleteexportprofileuievent" => new DeleteExportProfileUIEventConfig(),
                "deleteexportprofilefielduievent" => new DeleteExportProfileFieldUIEventConfig(),
                "runexportuievent" => new RunExportUIEventConfig(),
                "addexportscheduleuievent" => new AddExportScheduleUIEventConfig(),
                "editexportscheduleuievent" => new EditExportScheduleUIEventConfig(),
                "deleteexportscheduleuievent" => new DeleteExportScheduleUIEventConfig(),
                "viewexportprofilerecorduievent" => new ViewExportProfileRecordUIEventConfig(),
                "viewexporthistoryrecorduievent" => new ViewExportHistoryRecordUIEventConfig(),
                "manageexportprofilemappinguievent" => new ManageExportProfileMappingUIEventConfig(),
                _ => null,
            };
        }
    }
}
