using arc.common.Models.Config;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Builds the list of existing fields that may be referenced onto a target page, applying the
    /// view scope, entity scope and exclusion rules described in CONFIGURATION_SYSTEM.md.
    /// </summary>
    public interface IExistingFieldCatalogue
    {
        /// <summary>
        /// Builds the reuse candidates for a target page.
        /// </summary>
        /// <param name="viewConfigId">Configuration record id of the view the form was opened from.</param>
        /// <param name="targetFormName">Target form name id.</param>
        /// <param name="targetPageName">Target page name id.</param>
        /// <returns>The picker result, including the resolved target table and the options.</returns>
        Task<ExistingFieldQueryResultModel> BuildAsync(string viewConfigId, string targetFormName, string targetPageName);

        /// <summary>
        /// Resolves a single reference key to the source field configuration, or null when the source
        /// form, page or field no longer exists.
        /// </summary>
        /// <param name="referenceKey">Reference key <c>{form}|{page}|{fieldId}</c>.</param>
        /// <returns>The resolved candidate, or null.</returns>
        Task<ExistingFieldOptionModel> ResolveAsync(string referenceKey);

        /// <summary>
        /// Returns true when a page is eligible to receive referenced fields at all, so the client
        /// can hide the add existing field button rather than offer an empty picker. Pages with no
        /// entity table, and the tables excluded from reuse such as the test tables, are ineligible.
        /// </summary>
        /// <param name="formName">Form the page belongs to.</param>
        /// <param name="pageName">Page to test.</param>
        /// <returns>True when referenced fields may be added to the page.</returns>
        Task<bool> IsReuseAvailableAsync(string formName, string pageName);
    }
}
