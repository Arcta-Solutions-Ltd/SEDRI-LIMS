using arc.app.Graph;
using arc.common.ExtensionMethods;
using arc.common.Models.Home;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Resolves the logged-in user’s recent queue activity into specimen, patient, direct-test, and culture (isolate) rows
/// for the home dashboard Recently Used tile. Kind is derived only from normalized <c>queue.tablename</c>
/// (tests, specimen, patient, culture, and comment/tag table names); duplicates are collapsed in SQL (latest <c>added</c> per kind and entity).
/// Direct tests are filtered by canonical <c>Tests.TestName</c> when <c>recentlyuseddirecttestids</c> is set.
/// Culture (isolate) rows are filtered by <c>culture.typeid</c> (list item ids) when <c>recentlyusedculturetypeids</c> is non-empty.
/// Queue rows are always bounded in time: <c>startdate</c>/<c>enddate</c> from
/// <see cref="GraphDashboardTimeRangeResolver"/> (same rolling window as other home tiles). If those
/// parameters are absent, <see cref="GraphDashboardTimeRangeResolver.DefaultRollingUtcBoundsForMissingDashboardDates"/>
/// applies (1 year, matching the default new-section timerange).
/// If <c>startdate</c>/<c>enddate</c> are missing or degenerate, bounds are recomputed from <c>timerangeamount</c>/<c>timerangeunit</c>
/// (still present on the filter after <see cref="GraphDashboardTimeRangeResolver.Apply"/>).
/// Queue rows: pending (665) or completed (666).
/// After deduplicating by (kind, entity_id), the SQL keeps the top <c>perKindLimit</c> rows per <c>kind</c> by recency,
/// then the client applies the global <c>recentlyusedlimit</c> so less-active kinds are not starved by specimen/test volume.
/// </summary>
internal class HomeDashboardRecentlyUsedQuery : IQueryReturningType<List<HomeDashboardRecentItemModel>>
{
    /// <inheritdoc />
    public async Task<List<HomeDashboardRecentItemModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var username = queryFilters.GetStringValue("dashboardusername");
        if (string.IsNullOrWhiteSpace(username))
        {
            return [];
        }

        var kinds = ParseKinds(queryFilters.GetStringValue("recentlyusedkinds"));
        if (kinds.Count == 0)
        {
            kinds.Add("specimen");
            kinds.Add("patient");
            kinds.Add("test");
            kinds.Add("culture");
        }

        _ = queryFilters.TryParseIntegerValue("recentlyusedlimit", out var limit, 10);
        limit = Math.Clamp(limit, 1, 200);

        var allowedTestNames = ParseDirectTestNameFilters(queryFilters.GetStringValue("recentlyuseddirecttestids"));
        var allowedCultureTypeIds = ParseIdList(queryFilters.GetStringValue("recentlyusedculturetypeids"));

        DateTime start;
        DateTime end;
        var hasStart = queryFilters.TryParseDateValue("startdate", out var startDateParsed, default);
        var hasEnd = queryFilters.TryParseDateValue("enddate", out var endDateParsed, default);
        if (hasStart && hasEnd)
        {
            start = startDateParsed.Kind == DateTimeKind.Utc ? startDateParsed : startDateParsed.ToUniversalTime();
            end = endDateParsed.Kind == DateTimeKind.Utc ? endDateParsed : endDateParsed.ToUniversalTime();
            if (end < start)
            {
                (start, end) = (end, start);
            }
        }
        else
        {
            (start, end) = GraphDashboardTimeRangeResolver.DefaultRollingUtcBoundsForMissingDashboardDates();
        }

        // If the window is empty or near-zero (e.g. start and end both resolved to "now"), recompute from
        // dashboard rolling settings (timerangeamount / timerangeunit are left on the filter after Apply).
        if (end <= start || (end - start) < TimeSpan.FromMinutes(1))
        {
            _ = queryFilters.TryParseIntegerValue("timerangeamount", out var rollAmount, 1);
            if (rollAmount < 1)
            {
                rollAmount = 1;
            }

            queryFilters.TryGetStringValue("timerangeunit", out var rollUnit, "years");
            if (string.IsNullOrWhiteSpace(rollUnit))
            {
                rollUnit = "years";
            }

            (start, end) = GraphDashboardTimeRangeResolver.ComputeRollingUtcBounds(rollAmount, rollUnit);
        }

        var labCsv = queryFilters.GetStringValue("allowedlaboratories");
        if (string.IsNullOrWhiteSpace(labCsv))
        {
            labCsv = queryFilters.GetStringValue("laboratoryid");
        }
        var orgCsv = queryFilters.GetStringValue("allowedorganisations");
        if (string.IsNullOrWhiteSpace(orgCsv))
        {
            orgCsv = queryFilters.GetStringValue("organisationid");
        }

        var labIds = ParseIdList(labCsv);
        var orgIds = ParseIdList(orgCsv);

        // Pending (665) and completed (666). Kind from queue.tablename only; one row per (kind, entity_id) with latest added,
        // then top perKindLimit rows per kind so patient/culture are not crowded out by specimen/test activity.
        var perKindLimit = limit;
        var sql = """
            WITH base AS (
                SELECT q.id, q.added, trim(lower(q.tablename)) AS tn, q.recordid, q.specimenid, q.patientid
                FROM queue q
                WHERE lower(q.username) = lower(@username)
                  AND q.eventstatusid IN (665, 666)
                  AND q.added >= @start AND q.added <= @end
                  AND q.tablename IS NOT NULL
                  AND trim(q.tablename) <> ''
                  AND trim(lower(q.tablename)) IN (
                      'tests', 'specimen', 'patient', 'culture',
                      'specimencomment', 'specimentag', 'patientcomment', 'patienttag')
            ),
            classified AS (
                SELECT
                    b.id,
                    b.added,
                    CASE b.tn
                        WHEN 'tests' THEN 'test'
                        WHEN 'specimen' THEN 'specimen'
                        WHEN 'specimencomment' THEN 'specimen'
                        WHEN 'specimentag' THEN 'specimen'
                        WHEN 'patient' THEN 'patient'
                        WHEN 'patientcomment' THEN 'patient'
                        WHEN 'patienttag' THEN 'patient'
                        WHEN 'culture' THEN 'culture'
                        ELSE ''
                    END AS kind,
                    CASE
                        WHEN b.tn = 'tests' THEN NULLIF(b.recordid, 0)
                        WHEN b.tn = 'culture' THEN NULLIF(b.recordid, 0)
                        WHEN b.tn IN ('specimen', 'specimencomment', 'specimentag') THEN COALESCE(NULLIF(b.specimenid, 0), NULLIF(b.recordid, 0))
                        WHEN b.tn IN ('patient', 'patientcomment', 'patienttag') THEN COALESCE(NULLIF(b.patientid, 0), NULLIF(b.recordid, 0))
                        ELSE NULL
                    END AS entity_id
                FROM base b
            ),
            ranked AS (
                SELECT c.id, c.added, c.kind, c.entity_id,
                       ROW_NUMBER() OVER (PARTITION BY c.kind, c.entity_id ORDER BY c.added DESC) AS rn
                FROM classified c
                WHERE c.kind <> ''
                  AND c.entity_id IS NOT NULL
                  AND c.entity_id > 0
            ),
            deduped AS (
                SELECT r.id, r.added, r.kind, r.entity_id
                FROM ranked r
                WHERE r.rn = 1
            ),
            per_kind AS (
                SELECT d.id, d.added, d.kind, d.entity_id,
                       ROW_NUMBER() OVER (PARTITION BY d.kind ORDER BY d.added DESC) AS kind_rn
                FROM deduped d
            )
            SELECT p.added AS Added, p.kind AS Kind, p.entity_id AS EntityId
            FROM per_kind p
            WHERE p.kind_rn <= @perKindLimit
            ORDER BY p.added DESC
            LIMIT 4000
            """;

        var rows = (await connect.QueryAsync<DedupedQueueRow>(sql, new
        {
            username,
            start,
            end,
            perKindLimit,
        })).ToList();

        var ordered = new List<(string Kind, int EntityId, DateTime Added)>();

        foreach (var row in rows)
        {
            var kind = row.Kind ?? "";
            if (kind == "" || !kinds.Contains(kind))
            {
                continue;
            }

            ordered.Add((kind, row.EntityId, row.Added));
            if (ordered.Count >= limit)
            {
                break;
            }
        }

        var result = new List<HomeDashboardRecentItemModel>();

        var testIds = ordered.Where(x => x.Kind == "test").Select(x => x.EntityId).Distinct().ToList();
        var specimenIds = ordered.Where(x => x.Kind == "specimen").Select(x => x.EntityId).Distinct().ToList();
        var patientIds = ordered.Where(x => x.Kind == "patient").Select(x => x.EntityId).Distinct().ToList();
        var cultureIds = ordered.Where(x => x.Kind == "culture").Select(x => x.EntityId).Distinct().ToList();

        var testDetails = await LoadTestDetailsAsync(connect, testIds, allowedTestNames, labIds, orgIds);
        var specimenDetails = await LoadSpecimenDetailsAsync(connect, specimenIds, labIds, orgIds);
        var patientDetails = await LoadPatientDetailsAsync(connect, patientIds);
        var cultureDetails = await LoadCultureDetailsAsync(connect, cultureIds, labIds, orgIds, allowedCultureTypeIds);

        foreach (var item in ordered)
        {
            switch (item.Kind)
            {
                case "test":
                    if (testDetails.TryGetValue(item.EntityId, out var td))
                    {
                        result.Add(td);
                    }
                    break;
                case "specimen":
                    if (specimenDetails.TryGetValue(item.EntityId, out var specDetail))
                    {
                        result.Add(specDetail);
                    }
                    break;
                case "patient":
                    if (patientDetails.TryGetValue(item.EntityId, out var pd))
                    {
                        result.Add(pd);
                    }
                    break;
                case "culture":
                    if (cultureDetails.TryGetValue(item.EntityId, out var cd))
                    {
                        result.Add(cd);
                    }
                    break;
            }
        }

        return result;
    }

    private static HashSet<string> ParseKinds(string? csv)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(csv))
        {
            return set;
        }
        foreach (var p in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var k = p.ToLowerInvariant();
            if (k is "specimen" or "patient" or "test" or "culture")
            {
                set.Add(k);
            }
        }
        return set;
    }

    /// <summary>
    /// When non-empty, only <see cref="HomeDashboardRecentItemModel.TestName"/> values in this set are returned for test rows
    /// (dashboard section &quot;direct tests&quot; multiselect). Empty set means no name filter.
    /// </summary>
    private static HashSet<string> ParseDirectTestNameFilters(string? csv)
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(csv))
        {
            return names;
        }
        foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var idx = part.IndexOf('|', StringComparison.Ordinal);
            var formName = idx >= 0 && idx < part.Length - 1 ? part[(idx + 1)..] : part;
            if (!string.IsNullOrWhiteSpace(formName))
            {
                names.Add(formName.Trim());
            }
        }
        return names;
    }

    private static List<int> ParseIdList(string? csv)
    {
        var list = new List<int>();
        if (string.IsNullOrWhiteSpace(csv))
        {
            return list;
        }
        foreach (var s in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) && id > 0)
            {
                list.Add(id);
            }
        }
        return list;
    }

    private static async Task<Dictionary<int, HomeDashboardRecentItemModel>> LoadTestDetailsAsync(
        NpgsqlConnection connect,
        List<int> testIds,
        HashSet<string> allowedTestNames,
        List<int> labIds,
        List<int> orgIds)
    {
        var map = new Dictionary<int, HomeDashboardRecentItemModel>();
        if (testIds.Count == 0)
        {
            return map;
        }

        var sanitized = SqlSanitizer.SanitizeIdList(string.Join(",", testIds));
        var labFilter = labIds.Count > 0 ? " AND s.laboratoryid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", labIds)) + ") " : "";
        var orgFilter = orgIds.Count > 0 ? " AND s.organisationid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", orgIds)) + ") " : "";

        var sql = $"""
            SELECT t.id AS TestId, t.testname AS TestName, s.id AS SpecimenId, s.accessionnumber AS AccessionNumber,
                   s.specimentypeid AS SpecimenTypeId, p.id AS PatientId, p.patientref AS PatientRef,
                   p.firstname AS PatientFirstName, p.surname AS PatientSurname
            FROM tests t
            INNER JOIN specimen s ON s.id = t.specimenid
            INNER JOIN patient p ON p.id = s.patientid
            WHERE t.id IN ({sanitized})
            {labFilter}
            {orgFilter}
            """;

        var rows = await connect.QueryAsync<TestDetailRow>(sql);
        foreach (var r in rows)
        {
            if (allowedTestNames.Count > 0 && !allowedTestNames.Contains(r.TestName ?? ""))
            {
                continue;
            }

            map[r.TestId] = new HomeDashboardRecentItemModel
            {
                Kind = "test",
                TestId = r.TestId,
                TestName = r.TestName ?? "",
                SpecimenId = r.SpecimenId,
                PatientId = r.PatientId,
                AccessionNumber = r.AccessionNumber ?? "",
                SpecimenTypeId = r.SpecimenTypeId,
                PatientRef = r.PatientRef ?? "",
                PatientFirstName = r.PatientFirstName ?? "",
                PatientSurname = r.PatientSurname ?? "",
            };
        }

        return map;
    }

    private static async Task<Dictionary<int, HomeDashboardRecentItemModel>> LoadSpecimenDetailsAsync(
        NpgsqlConnection connect,
        List<int> specimenIds,
        List<int> labIds,
        List<int> orgIds)
    {
        var map = new Dictionary<int, HomeDashboardRecentItemModel>();
        if (specimenIds.Count == 0)
        {
            return map;
        }

        var sanitized = SqlSanitizer.SanitizeIdList(string.Join(",", specimenIds));
        var labFilter = labIds.Count > 0 ? " AND s.laboratoryid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", labIds)) + ") " : "";
        var orgFilter = orgIds.Count > 0 ? " AND s.organisationid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", orgIds)) + ") " : "";

        var sql = $"""
            SELECT s.id AS SpecimenId, s.accessionnumber AS AccessionNumber, s.specimentypeid AS SpecimenTypeId,
                   p.id AS PatientId, p.patientref AS PatientRef, p.firstname AS PatientFirstName, p.surname AS PatientSurname
            FROM specimen s
            INNER JOIN patient p ON p.id = s.patientid
            WHERE s.id IN ({sanitized})
            {labFilter}
            {orgFilter}
            """;

        var rows = await connect.QueryAsync<SpecimenDetailRow>(sql);
        foreach (var r in rows)
        {
            map[r.SpecimenId] = new HomeDashboardRecentItemModel
            {
                Kind = "specimen",
                SpecimenId = r.SpecimenId,
                PatientId = r.PatientId,
                AccessionNumber = r.AccessionNumber ?? "",
                SpecimenTypeId = r.SpecimenTypeId,
                PatientRef = r.PatientRef ?? "",
                PatientFirstName = r.PatientFirstName ?? "",
                PatientSurname = r.PatientSurname ?? "",
            };
        }

        return map;
    }

    private static async Task<Dictionary<int, HomeDashboardRecentItemModel>> LoadPatientDetailsAsync(NpgsqlConnection connect, List<int> patientIds)
    {
        var map = new Dictionary<int, HomeDashboardRecentItemModel>();
        if (patientIds.Count == 0)
        {
            return map;
        }

        var sanitized = SqlSanitizer.SanitizeIdList(string.Join(",", patientIds));
        var sql = $"""
            SELECT p.id AS PatientId, p.patientref AS PatientRef, p.firstname AS PatientFirstName, p.surname AS PatientSurname
            FROM patient p
            WHERE p.id IN ({sanitized})
            """;

        var rows = await connect.QueryAsync<PatientDetailRow>(sql);
        foreach (var r in rows)
        {
            map[r.PatientId] = new HomeDashboardRecentItemModel
            {
                Kind = "patient",
                PatientId = r.PatientId,
                PatientRef = r.PatientRef ?? "",
                PatientFirstName = r.PatientFirstName ?? "",
                PatientSurname = r.PatientSurname ?? "",
            };
        }

        return map;
    }

    private static async Task<Dictionary<int, HomeDashboardRecentItemModel>> LoadCultureDetailsAsync(
        NpgsqlConnection connect,
        List<int> cultureIds,
        List<int> labIds,
        List<int> orgIds,
        List<int> allowedCultureTypeIds)
    {
        var map = new Dictionary<int, HomeDashboardRecentItemModel>();
        if (cultureIds.Count == 0)
        {
            return map;
        }

        var sanitized = SqlSanitizer.SanitizeIdList(string.Join(",", cultureIds));
        var labFilter = labIds.Count > 0 ? " AND s.laboratoryid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", labIds)) + ") " : "";
        var orgFilter = orgIds.Count > 0 ? " AND s.organisationid IN (" + SqlSanitizer.SanitizeIdList(string.Join(",", orgIds)) + ") " : "";

        var sql = $"""
            SELECT c.id AS CultureId, c.typeid AS CultureTypeId, c.culturenumber AS CultureNumber,
                   s.id AS SpecimenId, s.accessionnumber AS AccessionNumber,
                   p.id AS PatientId, p.patientref AS PatientRef, p.firstname AS PatientFirstName, p.surname AS PatientSurname
            FROM culture c
            INNER JOIN specimen s ON s.id = c.specimenid
            INNER JOIN patient p ON p.id = s.patientid
            WHERE c.id IN ({sanitized})
            {labFilter}
            {orgFilter}
            """;

        var rows = await connect.QueryAsync<CultureDetailRow>(sql);
        foreach (var r in rows)
        {
            if (allowedCultureTypeIds.Count > 0)
            {
                if (!r.CultureTypeId.HasValue || !allowedCultureTypeIds.Contains(r.CultureTypeId.Value))
                {
                    continue;
                }
            }

            map[r.CultureId] = new HomeDashboardRecentItemModel
            {
                Kind = "culture",
                CultureId = r.CultureId,
                CultureTypeId = r.CultureTypeId,
                CultureNumber = r.CultureNumber,
                SpecimenId = r.SpecimenId,
                PatientId = r.PatientId,
                AccessionNumber = r.AccessionNumber ?? "",
                PatientRef = r.PatientRef ?? "",
                PatientFirstName = r.PatientFirstName ?? "",
                PatientSurname = r.PatientSurname ?? "",
            };
        }

        return map;
    }

    private sealed class DedupedQueueRow
    {
        public DateTime Added { get; set; }
        public string? Kind { get; set; }
        public int EntityId { get; set; }
    }

    private sealed class TestDetailRow
    {
        public int TestId { get; set; }
        public string? TestName { get; set; }
        public int SpecimenId { get; set; }
        public string? AccessionNumber { get; set; }
        public int? SpecimenTypeId { get; set; }
        public int PatientId { get; set; }
        public string? PatientRef { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientSurname { get; set; }
    }

    private sealed class SpecimenDetailRow
    {
        public int SpecimenId { get; set; }
        public string? AccessionNumber { get; set; }
        public int? SpecimenTypeId { get; set; }
        public int PatientId { get; set; }
        public string? PatientRef { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientSurname { get; set; }
    }

    private sealed class PatientDetailRow
    {
        public int PatientId { get; set; }
        public string? PatientRef { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientSurname { get; set; }
    }

    private sealed class CultureDetailRow
    {
        public int CultureId { get; set; }
        public int? CultureTypeId { get; set; }
        public int? CultureNumber { get; set; }
        public int SpecimenId { get; set; }
        public string? AccessionNumber { get; set; }
        public int PatientId { get; set; }
        public string? PatientRef { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientSurname { get; set; }
    }
}
