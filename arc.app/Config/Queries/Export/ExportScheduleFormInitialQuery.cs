using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Minimal query that returns ExportProfileId for the add export schedule form.
/// Used to populate the form with the parent ExportProfileId when opened from the embedded list Add button.
/// </summary>
internal class ExportScheduleFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the export schedule form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExportScheduleFormInitialQuery',
            'TableName': 'exportprofile',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int', 'KnownAs': 'ExportProfileId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
