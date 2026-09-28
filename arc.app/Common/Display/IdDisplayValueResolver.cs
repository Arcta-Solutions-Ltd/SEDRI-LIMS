using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Base for resolvers whose stored value is one id, or several ids separated by commas. Handles splitting,
    /// re-joining and caching so each concrete resolver only has to describe its lookup. Results are cached for
    /// the lifetime of the instance, which is one request, so a grid of twenty rows referring to the same few
    /// ids does not repeat the lookup twenty times.
    /// </summary>
    public abstract class IdDisplayValueResolver : IDisplayValueResolver
    {
        private readonly Dictionary<int, string> _cache = new();

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <summary>
        /// Looks up display text for ids that are not already cached.
        /// </summary>
        /// <param name="ids">Positive, distinct ids to look up.</param>
        /// <returns>Id to display text; ids with no match may be omitted.</returns>
        protected abstract Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids);

        /// <inheritdoc />
        public async Task<string> ResolveAsync(string rawValue)
        {
            var ids = ParseIds(rawValue);
            if (ids.Count == 0)
            {
                return "";
            }

            var missing = ids.Where(id => !_cache.ContainsKey(id)).Distinct().ToList();
            if (missing.Count > 0)
            {
                var found = await LookupAsync(missing);
                foreach (var id in missing)
                {
                    _cache[id] = found != null && found.TryGetValue(id, out var text) ? text : null;
                }
            }

            var resolved = ids
                .Select(id => _cache.TryGetValue(id, out var text) ? text : null)
                .Where(text => !string.IsNullOrWhiteSpace(text));

            return string.Join(", ", resolved);
        }

        /// <summary>
        /// Splits a stored value into the positive ids it contains.
        /// </summary>
        /// <param name="rawValue">Stored value; may be null, empty, the literal "null" or comma separated.</param>
        /// <returns>The ids in the order they appear, excluding zero, negatives and unparsable entries.</returns>
        private static List<int> ParseIds(string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue) || rawValue == "null")
            {
                return new List<int>();
            }

            return rawValue
                .Split(',')
                .Select(part => int.TryParse(part.Trim(), out var id) ? id : 0)
                .Where(id => id > 0)
                .ToList();
        }
    }
}
