using arc.common.Models.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Interface for selecting cultures for reporting.
    /// </summary>
    public interface ICultureSelector
    {
        /// <summary>
        /// Asynchronously gets the contents for the culture selector.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of CultureSelectorListModel objects.</returns>
        Task<List<CultureSelectorListModel>> GetContentsAsync(int specimenId);
    }
}
