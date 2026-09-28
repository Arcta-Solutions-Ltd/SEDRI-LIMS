using System.Threading.Tasks;
using arc.common.Models.Instruments.CustomImport;

namespace arc.app.Import
{
    /// <summary>
    /// Persists a single Custom-interface record. The whole record (patient, specimens, cultures, direct tests, isolate
    /// tests and AST rows) is saved inside one transaction so a failure at any level rolls back all writes for that
    /// record, keeping the load transactionally safe.
    /// </summary>
    public interface ICustomImportRepository
    {
        /// <summary>
        /// Upserts the supplied record inside a single transaction. Matching uses ids and the resolved reference values.
        /// Throws <see cref="arc.domain.Instruments.CustomImportException"/> when a business rule prevents the save (for
        /// example a new specimen whose patient does not exist and whose profile does not create patients).
        /// </summary>
        /// <param name="model">The resolved record to save.</param>
        /// <returns>The save result (ids and created/updated counts).</returns>
        Task<CustomImportSaveResult> SaveRecordAsync(CustomImportSaveModel model);
    }
}
