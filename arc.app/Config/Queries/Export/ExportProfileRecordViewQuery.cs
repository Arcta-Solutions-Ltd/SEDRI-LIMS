using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// This class defines the query to export profile record view.
/// Implements the IDefinition interface.
/// </summary>
internal class ExportProfileRecordViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query details as a JSON string.
    /// </summary>
    /// <returns>A JSON string containing the query details.</returns>
    public string Get()
    {
        return @"{ 
                    'Query': 'exportprofilerecordview',
                    'TableName': 'ExportProfileRecord',
                    'Type': 'Special'
                }";
    }
}
