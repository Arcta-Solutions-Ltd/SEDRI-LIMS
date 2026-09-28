using arc.common.Utils;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Helpers for matching isolate test configuration names irrespective of the "form" suffix
    /// and letter casing. Isolate tests are always matched on their configuration name, which is
    /// a stable identifier, and never on the translated title shown in the user interface.
    /// </summary>
    public static class IsolateTestNameExtensions
    {
        /// <summary>
        /// Normalizes an isolate test configuration name to its canonical form name.
        /// </summary>
        /// <param name="name">Configuration name from laboratory config, a form list or a selection key.</param>
        /// <returns>The normalized form name, or an empty string when the input is null or whitespace.</returns>
        public static string ToNormalizedFormName(this string name)
        {
            return CultureTestFormNameNormalizer.Normalize(name);
        }

        /// <summary>
        /// Determines whether a collection of isolate test configuration names contains the candidate name.
        /// </summary>
        /// <param name="names">Configuration names to search, in any casing and with or without the "form" suffix.</param>
        /// <param name="candidate">The isolate test configuration name to look for.</param>
        /// <returns>True when the candidate normalizes to one of the supplied names.</returns>
        public static bool ContainsFormName(this IReadOnlyCollection<string> names, string candidate)
        {
            var normalisedCandidate = candidate.ToNormalizedFormName();

            if (names == null || normalisedCandidate.Length == 0)
            {
                return false;
            }

            return names.Any(n => n.ToNormalizedFormName() == normalisedCandidate);
        }
    }
}
