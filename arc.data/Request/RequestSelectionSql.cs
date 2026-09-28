namespace arc.data.Request;

/// <summary>
/// Shared projection for the request selection screen. The descriptive request values live in the request
/// MoreData blob as ListItem ids, so each is resolved against ListItem to give the card readable text.
/// </summary>
internal static class RequestSelectionSql
{
    /// <summary>
    /// Select list and joins common to the by-patient and by-admission variants. Callers append their own
    /// where clause and the shared <see cref="OrderBy"/>.
    /// </summary>
    internal const string Projection = """
        SELECT r.Id,
               r.PatientId,
               r.AdmissionId,
               r.RequestId,
               r.MoreData::jsonb->>'RequestDate' AS RequestDate,
               r.MoreData::jsonb->>'RequestTime' AS RequestTime,
               ward.Value AS Ward,
               urgency.Value AS Urgency,
               indication.Value AS Indication,
               (SELECT COUNT(*) FROM Specimen s WHERE s.RequestId = r.Id) AS SpecimenCount,
               r.LastModifiedDate
        FROM Request r
        LEFT OUTER JOIN ListItem ward
            ON ward.Id = NULLIF(r.MoreData::jsonb->>'WardId', '')::int
        LEFT OUTER JOIN ListItem urgency
            ON urgency.Id = NULLIF(r.MoreData::jsonb->>'UrgencyId', '')::int
        LEFT OUTER JOIN ListItem indication
            ON indication.Id = NULLIF(r.MoreData::jsonb->>'IndicationId', '')::int
        """;

    /// <summary>
    /// Reverse chronological ordering used by both request selection queries.
    /// </summary>
    internal const string OrderBy = """
        ORDER BY NULLIF(r.MoreData::jsonb->>'RequestDate', '') DESC NULLS LAST, r.Id DESC
        """;
}
