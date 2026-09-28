using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Validation;

/// <summary>
/// Validates and sanitizes direct-test uievent names stored on list view <c>tests[]</c> arrays.
/// </summary>
public interface IListViewTestsValidator
{
    /// <summary>
    /// Validates whether a uievent name from <c>tests[]</c> resolves to a usable configuration.
    /// </summary>
    /// <param name="uiEventName">Stable uievent name (e.g. <c>gramstaintestuievent</c>), not a translated label.</param>
    /// <returns>The resolution outcome.</returns>
    Task<ListViewTestUiEventResolution> ValidateTestUiEventAsync(string uiEventName);

    /// <summary>
    /// Removes orphan entries from <paramref name="view"/>.<c>Tests</c> that cannot be resolved in code or DB.
    /// Does not persist changes to the database.
    /// </summary>
    /// <param name="view">The list view whose <c>tests[]</c> array should be sanitized.</param>
    Task SanitizeViewTestsAsync(ListViewConfig view);
}
