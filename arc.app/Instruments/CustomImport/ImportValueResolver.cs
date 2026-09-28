using System;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Common;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Default <see cref="IImportValueResolver"/>. Uses <see cref="CustomImportFieldRegistry"/> to decide whether a field
    /// is list-backed and, if so, resolves a display value to its list item id via <see cref="IListRepository"/>. All
    /// other values are returned trimmed. Resolution failures return the original value and are logged.
    /// </summary>
    public class ImportValueResolver : IImportValueResolver
    {
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportValueResolver"/> class.
        /// </summary>
        /// <param name="listRepository">List repository used to resolve list item display values to ids.</param>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public ImportValueResolver(IListRepository listRepository, ILogWriter logWriter)
        {
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<string> ResolveAsync(string bucket, string fieldName, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return rawValue;
            }

            var value = rawValue.Trim();
            var metadata = CustomImportFieldRegistry.Resolve(bucket, fieldName);
            if (metadata == null || metadata.Kind != ImportFieldKind.List || string.IsNullOrWhiteSpace(metadata.ListName))
            {
                return value;
            }

            // Already a numeric id: pass through so matching stays by id.
            if (int.TryParse(value, out _))
            {
                return value;
            }

            try
            {
                var options = await _listRepository.GetListValuesAsync(metadata.ListName, true);
                var match = options?.FirstOrDefault(o =>
                    string.Equals(o.Text?.Trim(), value, StringComparison.OrdinalIgnoreCase));
                if (match != null && !string.IsNullOrWhiteSpace(match.Key))
                {
                    return match.Key;
                }

                _logWriter.LogInfo(
                    $"Custom import: value '{value}' for field '{fieldName}' (bucket '{bucket}', list '{metadata.ListName}') could not be resolved to a list item id",
                    nameof(ImportValueResolver), nameof(ResolveAsync));
            }
            catch (Exception ex)
            {
                _logWriter.LogError(
                    $"Custom import: failed resolving list value '{value}' for field '{fieldName}' (list '{metadata.ListName}'): {ex.Message}",
                    nameof(ImportValueResolver), nameof(ResolveAsync));
            }

            return value;
        }
    }
}
