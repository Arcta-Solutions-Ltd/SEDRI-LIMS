using System.Text;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Writes an export as CSV. Each pipe-delimited row is converted to a quoted, comma-separated
    /// row. This is the default writer used when a profile has no structural mapping.
    /// </summary>
    public class CsvExportFormatWriter : IExportFormatWriter
    {
        /// <inheritdoc />
        public string Format => "csv";

        /// <inheritdoc />
        public string FileExtension => "csv";

        /// <inheritdoc />
        public string ContentType => "text/csv";

        /// <summary>
        /// Builds CSV content from the export lines (pipe-delimited rows converted to CSV format).
        /// </summary>
        /// <param name="context">The post-processed export context.</param>
        /// <returns>The CSV document.</returns>
        public string Write(ExportFormatContext context)
        {
            var sb = new StringBuilder();
            foreach (var row in context.Lines)
            {
                var csvRow = "\"" + row.Replace("|", "\",\"") + "\"";
                sb.AppendLine(csvRow);
            }
            return sb.ToString();
        }
    }
}
