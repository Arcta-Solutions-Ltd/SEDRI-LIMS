using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Cleans up an AST measurement for display. The AST page stores <c>-1</c> to mean "no measurement entered"
    /// for both zone diameters and MIC values, which should read as blank rather than as a negative number.
    /// Everything else, including MIC values that carry their comparison operator such as <c>&lt;=2</c>, is
    /// already display ready and passes through unchanged.
    /// </summary>
    public class AstMeasurementDisplayValueResolver : IDisplayValueResolver
    {
        private const string NoMeasurementSentinel = "-1";

        /// <inheritdoc />
        public string Name => "astmeasurement";

        /// <inheritdoc />
        public Task<string> ResolveAsync(string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue) || rawValue == "null")
            {
                return Task.FromResult("");
            }

            var trimmed = rawValue.Trim();

            return Task.FromResult(trimmed == NoMeasurementSentinel ? "" : trimmed);
        }
    }
}
