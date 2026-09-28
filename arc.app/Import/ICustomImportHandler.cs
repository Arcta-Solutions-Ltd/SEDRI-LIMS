using System.Threading.Tasks;
using arc.common.Models;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Instruments;

namespace arc.app.Import
{
    /// <summary>
    /// Turns a parsed Custom-interface <see cref="ImportRecord"/> into a resolved save model (list values resolved to ids,
    /// direct/isolate tests grouped by form) and persists it via <see cref="ICustomImportRepository"/>.
    /// </summary>
    public interface ICustomImportHandler
    {
        /// <summary>
        /// Builds and saves the record.
        /// </summary>
        /// <param name="record">The parsed record.</param>
        /// <param name="references">The resolved unique-reference fields per bucket.</param>
        /// <param name="profile">The Custom interface profile the record was loaded for.</param>
        /// <param name="token">Authenticated token (supplies laboratory/organisation for new specimens).</param>
        /// <returns>The save result.</returns>
        Task<CustomImportSaveResult> ImportAsync(
            ImportRecord record,
            arc.app.Instruments.CustomImport.UniqueReferenceMap references,
            SingleInstrumentConfig profile,
            TokenInfoModel token);
    }
}
