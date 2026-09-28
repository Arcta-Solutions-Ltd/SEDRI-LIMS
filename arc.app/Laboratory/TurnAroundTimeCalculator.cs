using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Laboratory;

/// <summary>
/// Calculates Turn Around Time (TAT) colour based on lab TAT configuration and elapsed time.
/// </summary>
public class TurnAroundTimeCalculator
{
    /// <summary>Specimen workflow state id meaning "finalised" for TAT end time from history.</summary>
    public const int SpecimenFinalisedStateId = 534;

    /// <summary>
    /// Calculates TAT colour for a specimen.
    /// Start = ReceivedDate + ReceivedTime, End = finalised from SpecimenStateHistory (StateId=534) or now.
    /// </summary>
    public static string GetSpecimenColour(DateTime? receivedDate, string receivedTime, DateTime? finalisedDate, JArray specimenRanges)
    {
        var elapsedMinutes = GetSpecimenTatElapsedMinutes(receivedDate, receivedTime, finalisedDate);
        return GetColourFromRanges(specimenRanges, elapsedMinutes);
    }

    /// <summary>
    /// Elapsed minutes for specimen TAT (same rules as <see cref="GetSpecimenColour"/> and home dashboard TAT compliance).
    /// </summary>
    public static double GetSpecimenTatElapsedMinutes(DateTime? receivedDate, string receivedTime, DateTime? finalisedDate)
    {
        var start = CombineDateAndTime(receivedDate, receivedTime);
        var end = finalisedDate ?? DateTime.UtcNow;

        start = ToUtcForTatComparison(start);
        end = ToUtcForTatComparison(end);

        if (end < start)
            end = DateTime.UtcNow;

        return (end - start).TotalMinutes;
    }

    /// <summary>
    /// Receipt + finalised timestamps must be compared in UTC. Unspecified Kind from CombineDateAndTime is treated as local wall-clock time.
    /// </summary>
    private static DateTime ToUtcForTatComparison(DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Utc) return dt;
        if (dt.Kind == DateTimeKind.Local) return dt.ToUniversalTime();
        return DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime();
    }

    /// <summary>
    /// Calculates TAT colour for a direct or culture test.
    /// Start = Requested, End = Completed if status Complete else now.
    /// </summary>
    public static string GetTestColour(
        DateTime requested,
        DateTime? completed,
        string status,
        string testName,
        bool isCultureTest,
        JArray defaultRanges,
        JObject overrides)
    {
        var end = (status?.Equals("Complete", StringComparison.OrdinalIgnoreCase) == true && completed.HasValue)
            ? completed.Value
            : DateTime.UtcNow;
        var elapsedMinutes = (end - requested).TotalMinutes;

        var ranges = GetRangesForTest(testName, defaultRanges, overrides);
        return GetColourFromRanges(ranges, elapsedMinutes);
    }

    /// <summary>
    /// Gets specimen finalised timestamp from SpecimenStateHistory (last row where StateId=534 and SpecimenId matches).
    /// </summary>
    public static DateTime? ParseFinalisedDate(object lastModifiedDate)
    {
        if (lastModifiedDate == null) return null;
        if (lastModifiedDate is DateTime dt) return dt;
        if (lastModifiedDate is DateTimeOffset dto) return dto.UtcDateTime;
        if (DateTime.TryParse(lastModifiedDate.ToString(), out var parsed)) return parsed;
        return null;
    }

    /// <summary>
    /// Combines date and time string (e.g. "09:30" or "1300") into a DateTime.
    /// </summary>
    public static DateTime CombineDateAndTime(DateTime? date, string time)
    {
        if (!date.HasValue) return DateTime.UtcNow;
        var (hours, minutes) = ParseTime(time);
        return date.Value.Date.AddHours(hours).AddMinutes(minutes);
    }

    private static (int hours, int minutes) ParseTime(string time)
    {
        if (string.IsNullOrWhiteSpace(time)) return (0, 0);
        var t = time.Trim();
        if (t.Length == 4 && int.TryParse(t, out var hhmm))
        {
            var h = hhmm / 100;
            var m = hhmm % 100;
            return (h, m);
        }
        var parts = t.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && int.TryParse(parts[0], out var hours) && int.TryParse(parts[1], out var minutes))
            return (hours, minutes);
        return (0, 0);
    }

    private static JArray GetRangesForTest(string testName, JArray defaultRanges, JObject overrides)
    {
        if (overrides != null && !string.IsNullOrEmpty(testName))
        {
            var testRanges = overrides[testName] as JArray;
            if (testRanges != null && testRanges.Count > 0)
                return testRanges;
        }
        return defaultRanges ?? new JArray();
    }

    private static string GetColourFromRanges(JArray ranges, double elapsedMinutes)
    {
        if (ranges == null || ranges.Count == 0) return null;

        foreach (var r in ranges)
        {
            var (minMinutes, maxMinutes) = GetRangeBoundsMinutes(r);
            if (minMinutes <= elapsedMinutes && elapsedMinutes <= maxMinutes)
            {
                return GetRangeString(r, "colour") ?? GetRangeString(r, "Colour");
            }
        }
        return null;
    }

    private static string GetRangeString(JToken range, string key)
    {
        var v = range[key];
        if (v == null && range is JObject obj)
            v = obj.Properties().FirstOrDefault(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase))?.Value;
        return v?.ToString();
    }

    private static bool HasRangeKey(JToken range, params string[] keys)
    {
        if (range is JObject obj)
        {
            foreach (var key in keys)
            {
                if (range[key] != null) return true;
                if (obj.Properties().Any(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase)))
                    return true;
            }
        }
        return false;
    }

    private static int GetRangeInt(JToken range, params string[] keys)
    {
        foreach (var key in keys)
        {
            var v = range[key];
            if (v == null && range is JObject obj)
                v = obj.Properties().FirstOrDefault(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase))?.Value;
            if (v != null && !string.IsNullOrWhiteSpace(v.ToString()) && int.TryParse(v.ToString(), out var n))
                return n;
        }
        return 0;
    }

    private static (double minMinutes, double maxMinutes) GetRangeBoundsMinutes(JToken range)
    {
        var hasMinMaxFormat = HasRangeKey(range, "maxDays", "MaxDays", "RangeToDays", "maxHours", "MaxHours", "RangeToHours", "maxMinutes", "MaxMinutes", "RangeToMinutes");
        if (hasMinMaxFormat)
        {
            var minD = GetRangeInt(range, "minDays", "MinDays", "RangeFromDays");
            var minH = GetRangeInt(range, "minHours", "MinHours", "RangeFromHours");
            var minM = GetRangeInt(range, "minMinutes", "MinMinutes", "RangeFromMinutes");
            var maxD = GetRangeInt(range, "maxDays", "MaxDays", "RangeToDays");
            var maxH = GetRangeInt(range, "maxHours", "MaxHours", "RangeToHours");
            var maxM = GetRangeInt(range, "maxMinutes", "MaxMinutes", "RangeToMinutes");
            var minMinutes = (minD * 24 * 60) + (minH * 60) + minM;
            var maxMinutes = (maxD * 24 * 60) + (maxH * 60) + maxM;
            return (minMinutes, maxMinutes);
        }
        var days = GetRangeInt(range, "days", "Days");
        var hours = GetRangeInt(range, "hours", "Hours");
        var minutes = GetRangeInt(range, "minutes", "Minutes");
        var upperBound = (days * 24 * 60) + (hours * 60) + minutes;
        return (0, upperBound);
    }
}
