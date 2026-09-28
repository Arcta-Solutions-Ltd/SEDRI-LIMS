using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Creates or links cultures/tests and inserts a pending <c>instrumentresults</c> row for a manually requested instrument profile.
/// </summary>
public interface IInstrumentManualRequestService
{
    /// <summary>
    /// Validates the selected profile for the navigation context, ensures prerequisite rows, and inserts a pending instrument result.
    /// </summary>
    /// <param name="dataToSave">JSON matching <see cref="arc.common.Models.Instruments.RequestInstrumentTestFormViewModel"/>.</param>
    /// <param name="username">Current user (for logging).</param>
    /// <returns>New <c>instrumentresults.id</c>.</returns>
    Task<int> RequestAsync(string dataToSave, string username);
}
