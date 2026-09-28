namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Resolves the <see cref="IExportFormatWriter"/> for a requested format. Adding a new format is
    /// a matter of implementing <see cref="IExportFormatWriter"/> and registering it here.
    /// </summary>
    public interface IExportFormatWriterFactory
    {
        /// <summary>
        /// Returns the writer for the supplied format. Unknown or empty formats fall back to CSV.
        /// </summary>
        /// <param name="format">The format key (<c>csv</c>, <c>json</c> or <c>xml</c>).</param>
        /// <returns>The matching <see cref="IExportFormatWriter"/>.</returns>
        IExportFormatWriter Create(string format);
    }
}
