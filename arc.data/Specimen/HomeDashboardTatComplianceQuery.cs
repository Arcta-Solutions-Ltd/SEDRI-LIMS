using arc.app.Graph;
using arc.app.Laboratory;
using arc.common.Models.Home;
using arc.data.Utils;
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
/// Computes TAT compliance for the home dashboard: specimens that are <b>currently</b> in Specimen Finalised state (534),
/// whose most recent transition to 534 falls in the primary time window. Matches
/// <see cref="SpecimenListTatEnricher"/> (TAT only when current state is 534) and
/// <see cref="SpecimenRepository.GetSpecimenFinalisedDatesAsync"/> for the finalised timestamp.
/// Specimen has no dedicated finalisation column; <c>SpecimenStateHistory</c> supplies <c>finalised_at</c> only for that end time.
/// </summary>
internal class HomeDashboardTatComplianceQuery : IQueryReturningType<TatComplianceKpiModel>
{
    private const int SpecimenFinalisedStateId = 534;

    /// <inheritdoc />
    public async Task<TatComplianceKpiModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var targetHours = ParseDoubleParam(queryFilters, "tattargethours", 48);
        if (targetHours <= 0)
        {
            targetHours = 48;
        }

        var lateThresholdHours = ParseDoubleParam(queryFilters, "tatlatethresholdhours", 48);
        if (lateThresholdHours <= 0)
        {
            lateThresholdHours = 48;
        }

        _ = queryFilters.TryParseIntegerValue("tataveragedays", out var averageDays, 0);
        if (averageDays < 0)
        {
            averageDays = 0;
        }

        var ragGreenMin = ParseDoubleParam(queryFilters, "raggreenmin", 90);
        var ragAmberMin = ParseDoubleParam(queryFilters, "ragambermin", 75);
        ragGreenMin = Math.Clamp(ragGreenMin, 0, 100);
        ragAmberMin = Math.Clamp(ragAmberMin, 0, 100);
        if (ragAmberMin > ragGreenMin)
        {
            ragAmberMin = ragGreenMin;
        }

        var (primaryStart, primaryEnd) = ResolvePrimaryWindow(queryFilters);

        var filterForSpecimen = CloneWithoutDashboardDateFilters(queryFilters);
        var dynamicWhere = await SpecimenFilter.GetWhereClauseAsync(filterForSpecimen, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(filterForSpecimen, connect);

        // Current state must be finalised (534); window applies to last transition to 534 (lf.finalised_at).
        const string stateAndWindow =
            "s.StateId = @stateId AND lf.finalised_at >= @winStart AND lf.finalised_at <= @winEnd";
        var whereClause = string.IsNullOrWhiteSpace(dynamicWhere)
            ? "WHERE " + stateAndWindow
            : $"{dynamicWhere} AND {stateAndWindow}";

        var primaryRows = await FetchFinalisedSpecimenRowsAsync(connect, whereClause, join, primaryStart, primaryEnd);
        var primary = AggregateWindow(primaryRows, targetHours, lateThresholdHours);

        double? rollingPct = null;
        if (averageDays > 0)
        {
            var rollEnd = DateTime.UtcNow;
            var rollStart = rollEnd.AddDays(-averageDays);
            var rollingRows = await FetchFinalisedSpecimenRowsAsync(connect, whereClause, join, rollStart, rollEnd);
            var rollingAgg = AggregateWindow(rollingRows, targetHours, lateThresholdHours);
            rollingPct = rollingAgg.TotalCount > 0
                ? Math.Round(100.0 * rollingAgg.OnTimeCount / rollingAgg.TotalCount, 1)
                : null;
        }

        var rag = ResolveRag(primary.PrimaryPercent, ragGreenMin, ragAmberMin);

        return new TatComplianceKpiModel
        {
            PrimaryPercent = primary.PrimaryPercent,
            OnTimeCount = primary.OnTimeCount,
            TotalCount = primary.TotalCount,
            LateCount = primary.LateCount,
            RollingAveragePercent = rollingPct,
            Rag = rag
        };
    }

    private static string ResolveRag(double? percent, double ragGreenMin, double ragAmberMin)
    {
        if (!percent.HasValue)
        {
            return "Red";
        }

        var p = percent.Value;
        if (p >= ragGreenMin)
        {
            return "Green";
        }

        if (p >= ragAmberMin)
        {
            return "Amber";
        }

        return "Red";
    }

    private static (DateTime Start, DateTime End) ResolvePrimaryWindow(QueryFilterConfig queryFilters)
    {
        var hasStart = queryFilters.TryParseDateValue("startdate", out var startDateParsed, default);
        var hasEnd = queryFilters.TryParseDateValue("enddate", out var endDateParsed, default);
        if (!hasEnd)
        {
            hasEnd = queryFilters.TryParseDateValue("endDate", out endDateParsed, default);
        }

        DateTime start;
        DateTime end;
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

        return (start, end);
    }

    private static QueryFilterConfig CloneWithoutDashboardDateFilters(QueryFilterConfig source)
    {
        var copy = new QueryFilterConfig
        {
            Name = source.Name,
            OrderBy = source.OrderBy,
            OrderDescending = source.OrderDescending,
            Parameters = []
        };
        foreach (var p in source.Parameters ?? [])
        {
            if (string.Equals(p.Key, "startdate", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (string.Equals(p.Key, "enddate", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (string.Equals(p.Key, "endDate", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Not applicable to specimen received→finalised TAT (graph filter rows are hidden in UI).
            if (string.Equals(p.Key, "stateid", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (string.Equals(p.Key, "testid", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            copy.Parameters.Add(new QueryValuesConfig { Key = p.Key, Value = p.Value });
        }

        return copy;
    }

    private static async Task<List<TatRow>> FetchFinalisedSpecimenRowsAsync(
        NpgsqlConnection connect,
        string whereClause,
        string join,
        DateTime winStart,
        DateTime winEnd)
    {
        var sql = $"""
            WITH last_final AS (
                SELECT DISTINCT ON (sh.SpecimenId) sh.SpecimenId, sh.LastModifiedDate AS finalised_at
                FROM SpecimenStateHistory sh
                WHERE sh.StateId = @stateId
                ORDER BY sh.SpecimenId, sh.Id DESC
            )
            SELECT s.Id AS SpecimenId, s.ReceivedDate, s.ReceivedTime::text AS ReceivedTime, lf.finalised_at AS FinalisedAt
            FROM specimen s
            INNER JOIN last_final lf ON lf.SpecimenId = s.Id
            {join}
            {whereClause}
            """;

        var rows = await connect.QueryAsync<TatRow>(sql, new
        {
            stateId = SpecimenFinalisedStateId,
            winStart,
            winEnd
        });
        return [.. rows];
    }

    private static (double? PrimaryPercent, int OnTimeCount, int TotalCount, int LateCount) AggregateWindow(
        IReadOnlyList<TatRow> rows,
        double targetHours,
        double lateThresholdHours)
    {
        var onTime = 0;
        var total = 0;
        var late = 0;

        foreach (var row in rows)
        {
            if (!row.ReceivedDate.HasValue)
            {
                continue;
            }

            var elapsedMinutes = TurnAroundTimeCalculator.GetSpecimenTatElapsedMinutes(
                row.ReceivedDate,
                row.ReceivedTime ?? "",
                row.FinalisedAt);
            var elapsedHours = elapsedMinutes / 60.0;
            if (double.IsNaN(elapsedHours) || elapsedHours < 0)
            {
                continue;
            }

            total++;
            if (elapsedHours <= targetHours)
            {
                onTime++;
            }

            if (elapsedHours > lateThresholdHours)
            {
                late++;
            }
        }

        double? pct = total > 0 ? Math.Round(100.0 * onTime / total, 1) : null;
        return (pct, onTime, total, late);
    }

    private static double ParseDoubleParam(QueryFilterConfig queryFilters, string key, double defaultValue)
    {
        if (!queryFilters.TryGetStringValue(key, out var s) || string.IsNullOrWhiteSpace(s))
        {
            return defaultValue;
        }

        return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var d) ? d : defaultValue;
    }

    private sealed class TatRow
    {
        public int SpecimenId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedTime { get; set; }
        public DateTime FinalisedAt { get; set; }
    }
}
