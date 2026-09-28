using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static methods to help with string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all special characters except for '_' and '-' from a string. Numbers and letters are not removed.
        /// </summary>
        /// <param name="str">String to remove the special characters from</param>
        /// <returns>String with the special characters removed</returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns true if a string is 'Yes', otherwise returns false.
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>True if the string passed as a parameter is 'Yes'</returns>
        public static bool IsYes(this string str)
        {
            return str == "Yes";
        }

        /// <summary>
        /// Converts a string to an integer. If the string is not a number, 0 is returned.
        /// </summary>
        /// <param name="value">String to check</param>
        /// <returns>An integer converted from the string passed as a parameter</returns>
        public static int ToInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Determines whether the specified string contains only numeric digits.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>
        /// <c>true</c> if the string represents an integer (contains only digits);
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && str.All(char.IsDigit);
        }

        /// <summary>
        /// Determines whether the specified string represents an integer 
        /// and is greater than the provided value.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="value">The integer value to compare against.</param>
        /// <returns>
        /// <c>true</c> if the string is a valid integer and greater than the specified value; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntegerGreaterThan(this string str, int value)
        {
            if (str.IsInteger())
            {
                return str.ToInt() > value;
            }
            return false;
        }

        /// <summary>
        /// Decides which of two strings to add to the existing string depending on whether the existing string is empty
        /// </summary>
        /// <param name="str">String to add the new string to</param>
        /// <param name="concatIfBlank">String to return if the existing string is blank</param>
        /// <param name="concatIfNotBlank">String to add to the end of the current string if the existing string is not blank</param>
        /// <returns>New string</returns>
        public static string ConcatWithBlankStringCheck(this string str, string concatIfBlank, string concatIfNotBlank)
        {
            return string.IsNullOrEmpty(str) ? concatIfBlank : string.Concat(str,concatIfNotBlank);
        }


        /// <summary>
        /// Converts an object's property values to a delimited string.
        /// </summary>
        /// <param name="obj">The object whose properties will be processed.</param>
        /// <param name="delim">The delimiter to insert between property values.</param>
        /// <returns>
        /// A string starting with the delimiter, followed by the object's property values separated by the delimiter.
        /// </returns>

        public static string DelimitedStringToObject(this object obj, string delim)
        {
            var properties = obj.GetType().GetProperties();
            var values = properties.Select(prop => prop.GetValue(obj, null)?.ToString() ?? string.Empty);
            return delim + string.Join(delim, values);
        }

        /// <summary>
        /// Removes tab characters from a string.
        /// </summary>
        /// <param name="str">The string from which tabs are to be removed.</param>
        /// <returns>A string with all tab characters removed.</returns>
        public static string RemoveTabs(this string str)
        {
            return str.Replace("\t", "");
        }

        /// <summary>
        /// Removes end-of-line (newline) characters from a string.
        /// </summary>
        /// <param name="str">The string from which end-of-line characters are to be removed.</param>
        /// <returns>A string with all end-of-line characters removed.</returns>
        public static string RemoveEndOfLineCharacters(this string str)
        {
            return str.Replace("\n", "").Replace("\r", "");
        }

        /// <summary>
        /// Extension method to check if a string contains only a comma-separated list of numbers.
        /// </summary>
        /// <param name="str">The input string to validate.</param>
        /// <returns>True if the string contains only numbers separated by commas; otherwise, false.</returns>
        public static bool ContainsOnlyListOfNumbers(this string str)
        {
            return Regex.IsMatch(str, @"^\d+(,\d+)*$");
        }


        /// <summary>
        /// Extracts all numeric characters from the input string and converts them into an integer.
        /// If no numeric characters are found, the method returns 0.
        /// </summary>
        /// <param name="input">The input string containing both numeric and non-numeric characters.</param>
        /// <returns>An integer produced by concatenating all digits from the input string.</returns>
        public static int ToNumeric(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));

            var digits = new string(input.Where(char.IsDigit).ToArray());
            return string.IsNullOrEmpty(digits) ? 0 : int.Parse(digits);
        }

        /// <summary>
        /// Determines whether a CSV string contains a specified value.
        /// Performs case-insensitive comparison by default.
        /// </summary>
        /// <param name="csv">The comma-separated string to search within.</param>
        /// <param name="search">The string to find in the CSV.</param>
        /// <param name="comparisonType">
        /// The type of string comparison to use (default is <see cref="StringComparison.OrdinalIgnoreCase"/>).
        /// </param>
        /// <returns><c>true</c> if the search string is found; otherwise, <c>false</c>.</returns>
        public static bool CsvContains(this string csv, string search, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(csv))
                return false;

            return csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(item => item.Trim())
                      .Any(item => string.Equals(item, search, comparisonType));
        }

        /// <summary>
        /// Determines whether a CSV string contains a numeric value.
        /// Converts the integer to a string before performing the search.
        /// </summary>
        /// <param name="csv">The comma-separated string to search within.</param>
        /// <param name="search">The numeric value to find in the CSV.</param>
        /// <returns><c>true</c> if the search value is found; otherwise, <c>false</c>.</returns>
        public static bool CsvContains(this string csv, int search)
        {
            return csv.CsvContains(search.ToString());
        }

        /// <summary>
        /// Splits the string by the specified delimiter and returns an array of trimmed substrings.
        /// </summary>
        /// <param name="source">The source string to split.</param>
        /// <param name="delimiter">The delimiter to use for splitting the string.</param>
        /// <returns>An array of substrings.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the source string is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the delimiter is null or empty.</exception>
        public static string[] ToStringArray(this string source, string delimiter)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(delimiter))
                throw new ArgumentException("Delimiter cannot be null or empty.", nameof(delimiter));

            return source.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToArray();
        }

        /// <summary>
        /// Adds a new item to a comma-separated string. If the original string is empty or null, 
        /// the new item is returned after trimming. Otherwise, the trimmed new item is appended 
        /// to the trimmed original string with a comma separator.
        /// </summary>
        /// <param name="source">The original comma-separated string.</param>
        /// <param name="newItem">The new item to add to the string.</param>
        /// <returns>A combined comma-separated string containing the new item.</returns>
        public static string AddToCommaSeparatedString(this string source, string newItem)
        {
            if (string.IsNullOrWhiteSpace(newItem))
                return source;

            if (string.IsNullOrWhiteSpace(source))
                return newItem.Trim();

            return $"{source.Trim()}, {newItem.Trim()}";
        }

        /// <summary>
        /// Extension method for string comparison, ignoring case sensitivity.
        /// </summary>
        /// <param name="source">The source string to compare.</param>
        /// <param name="comparisonValue">The target string to compare against.</param>
        /// <returns>True if the strings are equal (case-insensitive), otherwise false.</returns>
        public static bool IsSameAs(this string source, string comparisonValue)
        {
            return string.Equals(source, comparisonValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Extension method for case-insensitive string comparison.
        /// Returns true if the strings are not the same (ignoring case).
        /// </summary>
        /// <param name="source">The first string to compare.</param>
        /// <param name="comparisonValue">The second string to compare against.</param>
        /// <returns>True if the strings are different (case-insensitive), otherwise false.</returns>
        public static bool IsNotSameAs(this string source, string comparisonValue)
        {
            return ! string.Equals(source, comparisonValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether any item from the source comma-separated string
        /// exists in the target comma-separated string.
        /// </summary>
        /// <param name="source">The source comma-separated string.</param>
        /// <param name="target">The target comma-separated string to search within.</param>
        /// <returns>True if any item from source exists in target; otherwise, false.</returns>
        public static bool ContainsAnyItem(this string source, string target)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
                return false;

            var sourceItems = source.Split(',').Select(s => s.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var targetItems = target.Split(',').Select(s => s.Trim());

            return targetItems.Any(item => sourceItems.Contains(item));
        }

        /// <summary>
        /// Normalizes instrument-reported values for MTB PCR list lookups (trim; hyphen to space so e.g. DETECTED-MEDIUM matches DETECTED MEDIUM in listitem).
        /// </summary>
        public static string NormalizeInstrumentListValueForMtbPcr(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";
            return value.Trim().Replace('-', ' ');
        }

        /// <summary>
        /// Converts Windows-style backslash path separators to forward slashes. This is useful when a
        /// filesystem path (for example <c>C:\arc-storage</c>) is embedded inside a JSON string that is
        /// sent to the front end, because forward slashes avoid the need for JSON backslash escaping.
        /// </summary>
        /// <param name="path">The path to normalize. May be null or empty.</param>
        /// <returns>The path with all backslashes replaced by forward slashes, or an empty string when the input is null or whitespace.</returns>
        public static string ToForwardSlashPath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// Parses a date string to a nullable <see cref="DateTime"/> date-only value.
        /// Returns <see langword="null"/> when the value is null, empty, whitespace, or not parseable.
        /// </summary>
        /// <param name="value">The date string from form JSON (often ISO <c>yyyy-MM-dd</c> or locale format).</param>
        /// <returns>The parsed date at midnight, or <see langword="null"/> when absent or invalid.</returns>
        public static DateTime? ToNullableDateTime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var s = value.Trim();
            if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return dt.Date;
            }

            if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
            {
                return dt.Date;
            }

            if (DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
            {
                return dt.Date;
            }

            return null;
        }
    }
}



