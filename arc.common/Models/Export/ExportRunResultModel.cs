using System.Collections.Generic;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Envelope returned by the export run endpoint. Carries the resolved output format so the
    /// browser can download the file with the correct extension and MIME type.
    /// </summary>
    /// <remarks>
    /// For CSV exports the pipe-delimited <see cref="Rows"/> are returned so the client can apply
    /// locale date formatting and pipe-to-CSV conversion before download. For JSON and XML exports
    /// the fully-serialised document is returned in <see cref="Content"/> with ISO-style date
    /// strings; the client localises those dates before saving the downloaded file.
    /// </remarks>
    public class ExportRunResultModel
    {
        /// <summary>
        /// Gets or sets the resolved output format: <c>csv</c>, <c>json</c> or <c>xml</c>.
        /// </summary>
        public string Format { get; set; } = "csv";

        /// <summary>
        /// Gets or sets the suggested download file name (including extension).
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the serialised document content for JSON and XML exports. Null for CSV.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the pipe-delimited rows (header + data) for CSV exports. Null for JSON/XML.
        /// </summary>
        public List<string> Rows { get; set; }
    }
}
