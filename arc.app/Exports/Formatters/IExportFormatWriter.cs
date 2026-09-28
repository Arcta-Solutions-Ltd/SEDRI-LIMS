namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Serialises a post-processed export (pipe-delimited rows plus metadata) into a concrete
    /// output format. Implementations are selected by <see cref="IExportFormatWriterFactory"/>
    /// based on the profile's mapping format, so new formats can be added without touching the
    /// export run pipeline.
    /// </summary>
    public interface IExportFormatWriter
    {
        /// <summary>
        /// Gets the format key this writer produces (<c>csv</c>, <c>json</c> or <c>xml</c>).
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Gets the file extension (without the leading dot) used for the stored/downloaded file.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Gets the MIME content type used when uploading/downloading the file.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Serialises the supplied export context into the writer's output format.
        /// </summary>
        /// <param name="context">The post-processed rows and metadata to serialise.</param>
        /// <returns>The serialised document as a string.</returns>
        string Write(ExportFormatContext context);
    }
}
