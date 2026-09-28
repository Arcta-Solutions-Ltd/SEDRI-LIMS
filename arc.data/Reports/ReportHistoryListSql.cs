namespace arc.data.Reports;

/// <summary>
/// Shared SQL fragments for report history lists scoped through the specimen foreign key.
/// </summary>
internal static class ReportHistoryListSql
{
    /// <summary>
    /// Select list joining report history to specimen for patient, admission, or request scoped lists.
    /// </summary>
    internal const string SelectList = """
        SELECT DISTINCT a.AccessionNumber,
               s.Id,
               s.Name,
               s.ReportConfig,
               to_char(s.LastModifiedDate::DATE, 'yyyy-mm-dd') AS LastModifiedDate
        FROM ReportHistory s
        INNER JOIN Specimen a ON a.Id = s.SpecimenId
        """;

    /// <summary>
    /// Standard reverse-chronological ordering for report history grids.
    /// </summary>
    internal const string OrderByLastModifiedDate = """
        ORDER BY LastModifiedDate
        """;
}
