using System.Threading.Tasks;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Instruments;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Evaluates a Custom interface profile's interface criteria against a parsed inbound record to decide whether the
    /// profile should load the record. Matching is by id (list criteria compare list item ids; the imported value is
    /// resolved to an id first) so it is language independent. Empty criteria match everything.
    /// </summary>
    public interface IInterfaceCriteriaEvaluator
    {
        /// <summary>
        /// Returns true when the record satisfies the profile's interface criteria under its combination rule
        /// (987 = And, 988 = Or). When there are no criteria, returns true.
        /// </summary>
        /// <param name="profile">The Custom interface profile carrying <see cref="SingleInstrumentConfig.InterfaceCriteria"/>.</param>
        /// <param name="record">The parsed inbound record.</param>
        Task<bool> IsMatchAsync(SingleInstrumentConfig profile, ImportRecord record);
    }
}
