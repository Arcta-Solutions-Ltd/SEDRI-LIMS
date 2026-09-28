using arc.app.Common;

namespace arc.app.Config.Queries.Reports;

/// <summary>
/// Defines the JSON payload for querying unapproved report history entries by their identifier.
/// </summary>
/// <remarks>
/// The JSON returned by <see cref="Get"/> includes:
/// - Query: 'unapprovedreportslistbyidquery'
/// - TableName: 'ReportHistory'
/// - Type: 'Special'
/// </remarks>
internal class UnapprovedReportsListByIdQuery : IDefinition
{
    /// <summary>
    /// Gets the JSON definition for the unapproved reports list by ID query.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string specifying the query name, target table, and type.
    /// </returns>
    public string Get()
    {
        return @"{
            'Query': 'unapprovedreportslistbyidquery',
            'TableName': 'ReportHistory',
            'Type': 'Special'
        }";
    }
}
