using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Age components calculated from a date of birth to a reference date.
    /// </summary>
    public readonly struct AgeComponents
    {
        /// <summary>Whole years in the age interval.</summary>
        public int Years { get; init; }

        /// <summary>Remaining whole months after years are subtracted.</summary>
        public int Months { get; init; }

        /// <summary>Remaining whole days after months are subtracted.</summary>
        public int Days { get; init; }

        /// <summary>Remaining whole hours after days are subtracted.</summary>
        public int Hours { get; init; }
    }

    /// <summary>
    /// Calculates and formats patient age for display from date of birth.
    /// </summary>
    public static class AgeDisplayExtensions
    {
        /// <summary>
        /// Calculates age in years, months, days, and hours from a date of birth to a reference date.
        /// </summary>
        /// <param name="dateOfBirth">The patient's date of birth.</param>
        /// <param name="referenceDate">The date/time to measure age against.</param>
        /// <returns>Age components; all zero when the date of birth is after the reference date.</returns>
        public static AgeComponents CalculateAge(DateTime dateOfBirth, DateTime referenceDate)
        {
            var dob = dateOfBirth.Date;
            var refDate = referenceDate;

            var years = refDate.Year - dob.Year;
            var months = refDate.Month - dob.Month;
            var days = refDate.Day - dob.Day;
            var hours = refDate.Hour - dob.Hour;

            if (hours < 0)
            {
                hours += 24;
                days -= 1;
            }

            if (days < 0)
            {
                var prevMonth = new DateTime(refDate.Year, refDate.Month, 1).AddDays(-1);
                days += prevMonth.Day;
                months -= 1;
            }

            if (months < 0)
            {
                months += 12;
                years -= 1;
            }

            if (years < 0)
            {
                return new AgeComponents();
            }

            return new AgeComponents
            {
                Years = years,
                Months = months,
                Days = days,
                Hours = hours
            };
        }

        /// <summary>
        /// Formats age components as a display string with embedded language tags for translation.
        /// </summary>
        /// <param name="age">The age components to format.</param>
        /// <returns>
        /// A display string using singular or plural @GenYea@, @GenMon@, @GenDay@, and @GenHou@ tags,
        /// or an empty string when all components are zero.
        /// </returns>
        public static string FormatAgeDisplay(AgeComponents age)
        {
            if (age.Years > 0 || age.Months > 0)
            {
                var parts = new List<string>();
                if (age.Years > 0)
                {
                    parts.Add($"{age.Years} {UnitTag(age.Years, "@GenYeaC@", "@GenYeaA@")}");
                }

                if (age.Months > 0)
                {
                    parts.Add($"{age.Months} {UnitTag(age.Months, "@GenMonC@", "@GenMonA@")}");
                }

                return string.Join(" ", parts);
            }

            if (age.Days > 0 || (age.Days == 0 && age.Hours >= 24))
            {
                var dayCount = age.Days > 0 ? age.Days : 1;
                return $"{dayCount} {UnitTag(dayCount, "@GenDayB@", "@GenDayA@")}";
            }

            if (age.Hours > 0)
            {
                return $"{age.Hours} {UnitTag(age.Hours, "@GenHouB@", "@GenHouA@")}";
            }

            return string.Empty;
        }

        private static string UnitTag(int count, string singularTag, string pluralTag) =>
            count == 1 ? singularTag : pluralTag;

        /// <summary>
        /// Parses a date value from query result JSON (ISO or locale date string).
        /// </summary>
        /// <param name="value">The raw date string from a query result field.</param>
        /// <returns>A parsed <see cref="DateTime"/> or null when the value is missing or invalid.</returns>
        public static DateTime? TryParseQueryDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var iso))
            {
                return iso;
            }

            var slashParts = value.Trim().Split('/');
            if (slashParts.Length == 3
                && int.TryParse(slashParts[0], out var a)
                && int.TryParse(slashParts[1], out var b)
                && int.TryParse(slashParts[2], out var c))
            {
                if (a > 12)
                {
                    return new DateTime(c, b, a);
                }

                if (b > 12)
                {
                    return new DateTime(c, a, b);
                }

                return new DateTime(c, b, a);
            }

            return DateTime.TryParse(value, out var parsed) ? parsed : null;
        }

        /// <summary>
        /// Combines a date and optional time string into a single reference date/time.
        /// </summary>
        /// <param name="dateValue">The date portion.</param>
        /// <param name="timeValue">Optional time in HH:mm format.</param>
        /// <returns>The combined date/time, or null when the date is invalid.</returns>
        public static DateTime? CombineDateAndTime(string dateValue, string timeValue)
        {
            var date = TryParseQueryDate(dateValue);
            if (!date.HasValue)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(timeValue))
            {
                return date.Value;
            }

            var match = Regex.Match(timeValue.Trim(), @"^(\d{1,2}):(\d{2})");
            if (!match.Success)
            {
                return date.Value;
            }

            var hours = int.Parse(match.Groups[1].Value);
            var minutes = int.Parse(match.Groups[2].Value);
            return date.Value.Date.AddHours(hours).AddMinutes(minutes);
        }
    }
}
