using arc.common.Models.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Limits a culture (isolate) test form list to the tests applicable for the culture,
/// using the culture type and organism scope laboratory configuration.
/// </summary>
public interface ICultureTestSelectionFilter
{
    /// <summary>
    /// Removes isolate test forms that are not applicable for the given culture.
    /// Returns the list unchanged when no laboratory configuration applies to the culture.
    /// </summary>
    /// <param name="allTests">All isolate test forms available before filtering.</param>
    /// <param name="cultureId">Culture (isolate) record id.</param>
    /// <returns>The applicable subset of <paramref name="allTests"/>.</returns>
    Task<List<FormListModel>> ApplyAsync(List<FormListModel> allTests, string cultureId);
}
