using arc.app.Common;
using arc.domain.Configuration.QueryFiltersConfig;
using System;

namespace arc.app.Graph;

/// <summary>
/// Resolves home-dashboard time range parameters (<c>timerangemode</c> / rolling window) into
/// <c>startdate</c> and <c>enddate</c> on the query filter before graph or specimen count queries run.
/// Analytics requests that do not send <c>timerangemode</c> are unchanged.
/// </summary>
public static class GraphDashboardTimeRangeResolver
{
    /// <summary>
    /// When <c>timerangemode</c> is present, replaces any existing date bounds and sets
    /// <c>startdate</c>/<c>enddate</c> from the mode. Removes dashboard-only keys afterward.
    /// </summary>
    /// <param name="queryFilters">Request filter parameters.</param>
    /// <param name="logWriter">Logger for diagnostics.</param>
    public static void Apply(QueryFilterConfig queryFilters, ILogWriter logWriter)
    {
        if (queryFilters?.Parameters == null)
        {
            return;
        }

        if (!queryFilters.TryGetStringValue("timerangemode", out var mode, "") || string.IsNullOrWhiteSpace(mode))
        {
            return;
        }

        mode = mode.Trim().ToLowerInvariant();

        queryFilters.Remove("startdate");
        queryFilters.Remove("enddate");
        queryFilters.Remove("endDate");

        if (mode == "all")
        {
            logWriter.LogInfo("Dashboard time range: all (no date filter)", nameof(GraphDashboardTimeRangeResolver), nameof(Apply));
            StripDashboardKeys(queryFilters);
            return;
        }

        if (mode != "rolling")
        {
            StripDashboardKeys(queryFilters);
            return;
        }

        if (!queryFilters.TryParseIntegerValue("timerangeamount", out var amount, 1) || amount < 1)
        {
            amount = 1;
        }

        queryFilters.TryGetStringValue("timerangeunit", out var unit, "days");
        unit = unit.Trim().ToLowerInvariant();

        var (start, end) = ComputeRollingUtcBounds(amount, unit);

        queryFilters.AddString("startdate", start.ToString("o"));
        queryFilters.AddString("enddate", end.ToString("o"));

        logWriter.LogInfo(
            $"Dashboard time range: rolling amount={amount} unit={unit} start={start:o} end={end:o}",
            nameof(GraphDashboardTimeRangeResolver),
            nameof(Apply));

        StripDashboardKeys(queryFilters);
    }

    /// <summary>
    /// Computes UTC start and end for a rolling home-dashboard window.
    /// Same rules as <see cref="Apply"/> when <c>timerangemode</c> is <c>rolling</c> (end is always <see cref="DateTime.UtcNow"/>).
    /// </summary>
    /// <param name="amount">Positive window length; values under 1 are treated as 1.</param>
    /// <param name="unit">One of: hours, days, weeks, months, years (case-insensitive); unknown values fall back to days.</param>
    public static (DateTime Start, DateTime End) ComputeRollingUtcBounds(int amount, string unit)
    {
        if (amount < 1)
        {
            amount = 1;
        }

        unit = (unit ?? "days").Trim().ToLowerInvariant();

        var end = DateTime.UtcNow;
        var start = unit switch
        {
            "hours" => end.AddHours(-amount),
            "days" => end.AddDays(-amount),
            "weeks" => end.AddDays(-7 * amount),
            "months" => end.AddMonths(-amount),
            "years" => end.AddYears(-amount),
            _ => end.AddDays(-amount)
        };

        return (start, end);
    }

    /// <summary>
    /// Default rolling window when home dashboard sections do not supply timerange (matches a new section: 1 year).
    /// Used to bound queue scans when <c>startdate</c>/<c>enddate</c> are not present after <see cref="Apply"/>.
    /// </summary>
    public static (DateTime Start, DateTime End) DefaultRollingUtcBoundsForMissingDashboardDates()
    {
        return ComputeRollingUtcBounds(1, "years");
    }

    private static void StripDashboardKeys(QueryFilterConfig queryFilters)
    {
        // Keep timerangeamount / timerangeunit so downstream queries (e.g. home recently used) can
        // recompute a rolling window if startdate/enddate are missing or degenerate.
        queryFilters.Remove("timerangemode");
    }
}
