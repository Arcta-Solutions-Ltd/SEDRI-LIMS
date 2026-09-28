namespace arc.data.Instruments;

/// <summary>
/// SQL fragment for Vitek 2 LIS <c>collectedDateTime</c> (14 digits, specimen collection date/time).
/// </summary>
internal static class VitekCollectedDateTimeSql
{
    /// <summary>
    /// Produces <c>YYYYMMDDHH24MISS</c> from <c>specimen.collectiondate</c> and <c>collectiontime</c>, or <c>00000000000000</c> when date is null.
    /// </summary>
    internal const string SelectExpression =
        "CASE WHEN s.collectiondate IS NULL THEN '00000000000000' ELSE to_char((s.collectiondate::date + coalesce(s.collectiontime::time, time '00:00:00'))::timestamp, 'YYYYMMDDHH24MISS') END AS collecteddatetime";
}
