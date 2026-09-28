using System;
using arc.app.Common;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Default <see cref="IExportFormatWriterFactory"/>. Constructs the CSV, JSON and XML writers and
    /// selects one by format key, falling back to CSV when the format is unknown.
    /// </summary>
    public class ExportFormatWriterFactory : IExportFormatWriterFactory
    {
        private readonly IExportDocumentBuilder _documentBuilder;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportFormatWriterFactory"/> class.
        /// </summary>
        /// <param name="documentBuilder">Shared builder used by the JSON and XML writers.</param>
        /// <param name="logWriter">Log writer for diagnostics.</param>
        public ExportFormatWriterFactory(IExportDocumentBuilder documentBuilder, ILogWriter logWriter)
        {
            _documentBuilder = documentBuilder;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public IExportFormatWriter Create(string format)
        {
            switch ((format ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "json":
                    return new JsonExportFormatWriter(_documentBuilder);
                case "xml":
                    return new XmlExportFormatWriter(_documentBuilder);
                case "csv":
                    return new CsvExportFormatWriter();
                default:
                    _logWriter.LogInfo($"WARN: unknown export format '{format}', defaulting to csv", nameof(ExportFormatWriterFactory), nameof(Create));
                    return new CsvExportFormatWriter();
            }
        }
    }
}
