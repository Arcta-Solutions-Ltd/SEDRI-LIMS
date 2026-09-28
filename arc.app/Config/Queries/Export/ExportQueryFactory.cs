using arc.app.Common;
using arc.app.Config.Queries.Export;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Factory that resolves an export-area query configuration by its lower-case name.
    /// New export queries (including <c>exportprofilemappingforprofile</c>) are registered here.
    /// </summary>
    internal class ExportQueryFactory : IDefinitionFactory
    {
        /// <summary>
        /// Returns the <see cref="IDefinition"/> for an export query, or <c>null</c> when the name is unknown.
        /// </summary>
        /// <param name="definitionName">The query configuration name (case-insensitive).</param>
        /// <returns>The matching configuration definition, or <c>null</c> if no match is found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                //"exportlist" => new ExportListQuery(),
                "exportprofilelist" => new ExportProfileListQuery(),
                "exporthistorylist" => new ExportHistoryListQuery(),
                "exporthistoryattachmentsforrecordview" => new ExportHistoryAttachmentsForRecordViewQuery(),
                "exporthistoryrecordview" => new ExportHistoryRecordViewQuery(),
                "editexportprofile" => new EditExportProfileQuery(),
                "exportprofilebyid" => new ExportProfileByIdQuery(),
                "exportprofilefieldbyid" => new ExportProfileFieldByIdQuery(),
                "editexportprofilefieldsbyexportprofileid" => new EditExportProfileFieldsByExportProfileIdQuery(),
                "exportprofileview" => new ExportProfileForExportProfileViewQuery(),
                "exportprofilerecordview" => new ExportProfileRecordViewQuery(),
                "exportprofilealreadyexistsforadd" => new ExportProfileAlreadyExistsForAddQuery(),
                "exportschedulealreadyexistsforadd" => new ExportScheduleAlreadyExistsForAddQuery(),
                "exportschedulelistbyprofileid" => new ExportScheduleListByProfileIdQuery(),
                "exportscheduleforminitialquery" => new ExportScheduleFormInitialQuery(),
                "editexportschedule" => new EditExportScheduleQuery(),
                "exportprofilemappingforprofile" => new ExportProfileMappingForProfileQueryConfig(),
                _ => null,
            };
        }
    }
}
