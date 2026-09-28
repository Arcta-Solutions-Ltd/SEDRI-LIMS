using arc.common.Models.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Interface for selecting direct testsfor reporting.
    /// </summary>
    public interface IDirectTestSelector
    {
        /// <summary>
        /// Asynchronously gets the contents for the direct test selector.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of DirectTestSelectorListModel objects.</returns>
        Task<List<DirectTestSelectorListModel>> GetContentsAsync(int specimenId);
    }
}
