using arc.app.Common;
using arc.app.Config;
using arc.app.Config.Events;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.app.Config.UIEvents;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Builds the list of existing fields that may be referenced onto a target page.
    /// </summary>
    /// <remarks>
    /// Reuse is keyed on the entity table a page stores to, resolved by
    /// <see cref="FieldReuseScopeExtensions.ResolveScope"/>. Candidates are every field on a page
    /// resolving to that same table, on any configurable form, in any view. This is what makes
    /// patient, admission and request fields captured on the specimen view create forms available
    /// on the patient view, and what stops a specimen page offering patient fields.
    /// <para>
    /// The rule is intentionally generic rather than a list of known tables, so opening form
    /// configuration up to further views needs no change here. Two cases yield no candidates at
    /// all: pages with no entity table, which have nothing to share, and the tables listed in
    /// <see cref="FieldReuseCatalogue.NonReusableTables"/>, which is how direct test and culture
    /// test forms are kept out.
    /// </para>
    /// <para>
    /// Only fields already on the target <em>page</em> are withheld, not the whole form, so a
    /// definition can be copied onto a second page of the same form and made read only there for
    /// reference. Search, selection and results pages never contribute candidates because their
    /// fields are query criteria with no column and no MoreData key.
    /// </para>
    /// </remarks>
    public class ExistingFieldCatalogue : IExistingFieldCatalogue
    {
        private readonly IConfigRepository _configRepository;
        private readonly IListViewConfigFactory _listViewFactory;
        private readonly IUIEventConfigAdapter _uiEventAdapter;
        private readonly IFormConfigAdapter _formAdapter;
        private readonly IPageConfigAdapter _pageAdapter;
        private readonly IEventAdapter _eventAdapter;
        private readonly ILogWriter _logWriter;

        private const string ClassName = nameof(ExistingFieldCatalogue);
        private const string ViewConfigTypeId = "5";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistingFieldCatalogue"/> class.
        /// </summary>
        /// <param name="configRepository">Repository used to enumerate view configuration rows.</param>
        /// <param name="listViewFactory">Factory used to resolve list view definitions.</param>
        /// <param name="uiEventAdapter">Adapter used to resolve UI events to form names.</param>
        /// <param name="formAdapter">Adapter used to load form definitions.</param>
        /// <param name="pageAdapter">Adapter used to load page definitions.</param>
        /// <param name="eventAdapter">Adapter used to resolve a form save event table name.</param>
        /// <param name="logWriter">Structured log writer.</param>
        public ExistingFieldCatalogue(
            IConfigRepository configRepository,
            IListViewConfigFactory listViewFactory,
            IUIEventConfigAdapter uiEventAdapter,
            IFormConfigAdapter formAdapter,
            IPageConfigAdapter pageAdapter,
            IEventAdapter eventAdapter,
            ILogWriter logWriter)
        {
            _configRepository = configRepository;
            _listViewFactory = listViewFactory;
            _uiEventAdapter = uiEventAdapter;
            _formAdapter = formAdapter;
            _pageAdapter = pageAdapter;
            _eventAdapter = eventAdapter;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Builds the reuse candidates for a target page.
        /// </summary>
        /// <param name="viewConfigId">Configuration record id of the view the form was opened from.</param>
        /// <param name="targetFormName">Target form name id.</param>
        /// <param name="targetPageName">Target page name id.</param>
        /// <returns>The picker result, including the resolved target table and the options.</returns>
        public async Task<ExistingFieldQueryResultModel> BuildAsync(
            string viewConfigId,
            string targetFormName,
            string targetPageName)
        {
            var result = new ExistingFieldQueryResultModel
            {
                Id = $"{targetFormName}|{targetPageName}"
            };

            var targetForm = await _formAdapter.GetFormAsync(targetFormName);
            if (targetForm == null)
            {
                _logWriter?.LogError(
                    $"Target form {targetFormName} could not be resolved while building existing field candidates",
                    ClassName,
                    nameof(BuildAsync));
                return result;
            }

            var targetPage = await _pageAdapter.GetPageAsync(targetPageName);
            if (targetPage == null)
            {
                _logWriter?.LogError(
                    $"Target page {targetPageName} could not be resolved while building existing field candidates",
                    ClassName,
                    nameof(BuildAsync));
                return result;
            }

            result.TargetPageTitle = string.IsNullOrWhiteSpace(targetPage.PageTitle)
                ? targetPage.Name
                : targetPage.PageTitle;

            var targetSaveTable = await GetSaveEventTableNameAsync(targetForm);
            var targetScope = FieldReuseScopeExtensions.ResolveScope(
                targetPage.TableName,
                targetForm.SingleItemName,
                targetSaveTable);
            result.TargetTable = targetScope;

            _logWriter?.LogInfo(
                $"Existing field scope: view={viewConfigId}, form={targetFormName}, page={targetPageName}, targetTable={targetScope ?? "none"}",
                ClassName,
                nameof(BuildAsync));

            if (!FieldReuseCatalogue.IsReusableTable(targetScope))
            {
                _logWriter?.LogInfo(
                    $"Existing field reuse not available: form={targetFormName}, page={targetPageName}, targetTable={targetScope ?? "none"}, reason=tableNotReusable",
                    ClassName,
                    nameof(BuildAsync));
                return result;
            }

            var fieldIdsAlreadyOnPage = EnumerateFields(targetPage)
                .Select(f => f.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var candidateFormNames = await GetCandidateFormNamesAsync(
                viewConfigId,
                targetFormName,
                targetScope);

            var options = new List<ExistingFieldOptionModel>();
            var seenFieldIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var formsScanned = 0;
            var pagesScanned = 0;
            var rawFieldCount = 0;
            var afterScopeCount = 0;

            foreach (var candidateFormName in candidateFormNames)
            {
                var candidateForm = await _formAdapter.GetFormAsync(candidateFormName);
                if (candidateForm?.Pages == null)
                {
                    continue;
                }

                formsScanned++;
                var candidateSaveTable = await GetSaveEventTableNameAsync(candidateForm);

                foreach (var candidatePageName in candidateForm.Pages)
                {
                    var candidatePage = await _pageAdapter.GetPageAsync(candidatePageName);
                    if (candidatePage?.Columns == null)
                    {
                        continue;
                    }

                    pagesScanned++;

                    if (!FieldReuseCatalogue.IsCandidateSourcePage(candidatePage.TableName))
                    {
                        _logWriter?.LogInfo(
                            $"Existing field source page skipped: page={candidatePageName}, reason=noTable",
                            ClassName,
                            nameof(BuildAsync));
                        continue;
                    }

                    var candidateScope = FieldReuseScopeExtensions.ResolveScope(
                        candidatePage.TableName,
                        candidateForm.SingleItemName,
                        candidateSaveTable);

                    if (!FieldReuseScopeExtensions.ScopesMatch(targetScope, candidateScope))
                    {
                        continue;
                    }

                    var pageTitle = string.IsNullOrWhiteSpace(candidatePage.PageTitle)
                        ? candidatePage.Name
                        : candidatePage.PageTitle;

                    foreach (var field in EnumerateFields(candidatePage))
                    {
                        rawFieldCount++;

                        if (!FieldReuseCatalogue.IsReusable(field.Id, field.Type))
                        {
                            LogExclusion(field.Id, "reserved or non data type");
                            continue;
                        }

                        afterScopeCount++;

                        if (fieldIdsAlreadyOnPage.Contains(field.Id))
                        {
                            LogExclusion(field.Id, "alreadyOnPage");
                            continue;
                        }

                        if (!seenFieldIds.Add(field.Id))
                        {
                            LogExclusion(field.Id, "duplicate");
                            continue;
                        }

                        options.Add(new ExistingFieldOptionModel
                        {
                            Key = FieldReuseScopeExtensions.BuildFieldReferenceKey(
                                candidateFormName,
                                candidatePage.Name,
                                field.Id),
                            Text = string.IsNullOrWhiteSpace(field.Label) ? field.Id : field.Label,
                            FieldId = field.Id,
                            FieldType = field.Type,
                            TableName = candidateScope,
                            SourceForm = candidateFormName,
                            SourcePage = candidatePage.Name,
                            SourcePageTitle = pageTitle
                        });
                    }
                }
            }

            result.ExistingFieldOptions = options
                .OrderBy(o => o.SourcePageTitle, StringComparer.OrdinalIgnoreCase)
                .ThenBy(o => o.Text, StringComparer.OrdinalIgnoreCase)
                .ToList();

            _logWriter?.LogInfo(
                $"Existing field candidates: forms scanned={formsScanned}, pages scanned={pagesScanned}, raw={rawFieldCount}, afterScope={afterScopeCount}, afterExclusions={result.ExistingFieldOptions.Count}",
                ClassName,
                nameof(BuildAsync));

            if (result.ExistingFieldOptions.Count == 0)
            {
                _logWriter?.LogInfo(
                    $"Existing field candidates empty for form={targetFormName}, page={targetPageName}, targetTable={targetScope ?? "none"}",
                    ClassName,
                    nameof(BuildAsync));
            }

            return result;
        }

        /// <summary>
        /// Resolves a single reference key to the source field configuration, or null when the source
        /// form, page or field no longer exists.
        /// </summary>
        /// <param name="referenceKey">Reference key <c>{form}|{page}|{fieldId}</c>.</param>
        /// <returns>The resolved candidate, or null.</returns>
        public async Task<ExistingFieldOptionModel> ResolveAsync(string referenceKey)
        {
            var (formName, pageName, fieldId) =
                FieldReuseScopeExtensions.ParseFieldReferenceKey(referenceKey);

            if (formName == null)
            {
                _logWriter?.LogError(
                    $"Existing field reference unresolved: key={referenceKey}, missing=malformedKey",
                    ClassName,
                    nameof(ResolveAsync));
                return null;
            }

            var form = await _formAdapter.GetFormAsync(formName);
            if (form == null)
            {
                _logWriter?.LogError(
                    $"Existing field reference unresolved: key={referenceKey}, missing=form",
                    ClassName,
                    nameof(ResolveAsync));
                return null;
            }

            var page = await _pageAdapter.GetPageAsync(pageName);
            if (page?.Columns == null)
            {
                _logWriter?.LogError(
                    $"Existing field reference unresolved: key={referenceKey}, missing=page",
                    ClassName,
                    nameof(ResolveAsync));
                return null;
            }

            var field = EnumerateFields(page)
                .FirstOrDefault(f => f.Id.Equals(fieldId, StringComparison.OrdinalIgnoreCase));

            if (field == null)
            {
                _logWriter?.LogError(
                    $"Existing field reference unresolved: key={referenceKey}, missing=field",
                    ClassName,
                    nameof(ResolveAsync));
                return null;
            }

            var saveTable = await GetSaveEventTableNameAsync(form);
            var scope = FieldReuseScopeExtensions.ResolveScope(
                page.TableName,
                form.SingleItemName,
                saveTable);

            return new ExistingFieldOptionModel
            {
                Key = referenceKey,
                Text = string.IsNullOrWhiteSpace(field.Label) ? field.Id : field.Label,
                FieldId = field.Id,
                FieldType = field.Type,
                TableName = scope,
                SourceForm = formName,
                SourcePage = page.Name,
                SourcePageTitle = string.IsNullOrWhiteSpace(page.PageTitle) ? page.Name : page.PageTitle
            };
        }

        /// <summary>
        /// Returns true when a page is eligible to receive referenced fields at all, so the client
        /// can hide the add existing field button rather than offer an empty picker. Pages with no
        /// entity table, and the tables excluded from reuse such as the test tables, are ineligible.
        /// </summary>
        /// <param name="formName">Form the page belongs to.</param>
        /// <param name="pageName">Page to test.</param>
        /// <returns>True when referenced fields may be added to the page.</returns>
        public async Task<bool> IsReuseAvailableAsync(string formName, string pageName)
        {
            var form = await _formAdapter.GetFormAsync(formName);
            var page = await _pageAdapter.GetPageAsync(pageName);

            if (form == null || page == null)
            {
                return false;
            }

            var scope = FieldReuseScopeExtensions.ResolveScope(
                page.TableName,
                form.SingleItemName,
                await GetSaveEventTableNameAsync(form));

            return FieldReuseCatalogue.IsReusableTable(scope);
        }

        /// <summary>
        /// Builds the ordered, de-duplicated list of form names that may contribute candidates.
        /// The current view is listed first so its fields sort earliest, then every other view is
        /// added: a field belongs to its entity, not to the view it happens to be captured on.
        /// </summary>
        /// <param name="viewConfigId">Configuration record id of the current view.</param>
        /// <param name="targetFormName">Target form name id.</param>
        /// <param name="targetScope">Resolved target entity table.</param>
        /// <returns>Candidate form names.</returns>
        private async Task<List<string>> GetCandidateFormNamesAsync(
            string viewConfigId,
            string targetFormName,
            string targetScope)
        {
            var formNames = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void AddRange(IEnumerable<string> names)
            {
                foreach (var name in names)
                {
                    if (!string.IsNullOrWhiteSpace(name) && seen.Add(name))
                    {
                        formNames.Add(name);
                    }
                }
            }

            var currentViewName = await GetViewNameByConfigIdAsync(viewConfigId);
            if (currentViewName != null)
            {
                AddRange(await GetConfigurableFormNamesForViewAsync(currentViewName));
            }

            var beforeCrossView = formNames.Count;
            var viewsScanned = 0;

            var parameters = new QueryFilterConfig();
            parameters.AddString("ConfigTypeId", ViewConfigTypeId);
            var viewRows = await _configRepository.GetConfigListAsync(parameters);

            foreach (var viewRow in viewRows ?? [])
            {
                if (viewRow?.ConfigName == null
                    || viewRow.ConfigName.Equals(currentViewName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                viewsScanned++;
                AddRange(await GetConfigurableFormNamesForViewAsync(viewRow.ConfigName));
            }

            _logWriter?.LogInfo(
                $"Cross-view augmentation for table={targetScope}: views scanned={viewsScanned}, extra candidates={formNames.Count - beforeCrossView}",
                ClassName,
                nameof(GetCandidateFormNamesAsync));

            AddRange([targetFormName]);

            return formNames;
        }

        /// <summary>
        /// Resolves the list view configuration name for a view configuration record id.
        /// </summary>
        /// <param name="viewConfigId">Configuration record id.</param>
        /// <returns>The view configuration name, or null when the record cannot be read.</returns>
        private async Task<string> GetViewNameByConfigIdAsync(string viewConfigId)
        {
            if (string.IsNullOrWhiteSpace(viewConfigId))
            {
                return null;
            }

            try
            {
                var parameters = new QueryFilterConfig();
                parameters.AddString("id", viewConfigId);
                var viewRecord = await _configRepository.SingleConfigByIdAsync(parameters);
                return viewRecord?.ConfigName;
            }
            catch (Exception ex)
            {
                _logWriter?.LogError(
                    $"View config id {viewConfigId} could not be resolved while building existing field candidates: {ex.Message}",
                    ClassName,
                    nameof(GetViewNameByConfigIdAsync));
                return null;
            }
        }

        /// <summary>
        /// Returns the configurable form names reachable from a view, using the same UI event
        /// traversal as the Forms region of the view configuration screen.
        /// </summary>
        /// <param name="viewName">List view configuration name.</param>
        /// <returns>Configurable form names for that view.</returns>
        private async Task<List<string>> GetConfigurableFormNamesForViewAsync(string viewName)
        {
            var formNames = new List<string>();

            var view = await _listViewFactory.GetViewAsync(viewName);
            if (view == null)
            {
                _logWriter?.LogError(
                    $"View config {viewName} could not be resolved while building existing field candidates",
                    ClassName,
                    nameof(GetConfigurableFormNamesForViewAsync));
                return formNames;
            }

            var uiEvents = new List<string>();
            uiEvents.AddRange(view.GetThisLevelOnlyUIEventList() ?? []);
            uiEvents.AddRange(view.GetUIEventList() ?? []);

            foreach (var uiEventName in uiEvents.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var uiEvent = await _uiEventAdapter.GetEventAsync(uiEventName);
                if (uiEvent?.Type == null
                    || !uiEvent.Type.Equals("form", StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrWhiteSpace(uiEvent.Action))
                {
                    continue;
                }

                var form = await _formAdapter.GetFormAsync(uiEvent.Action);
                if (form?.Configurable != null
                    && form.Configurable.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    formNames.Add(uiEvent.Action);
                }
            }

            return formNames;
        }

        /// <summary>
        /// Resolves the table name of a form's save event, used as the last step of scope resolution.
        /// </summary>
        /// <param name="form">Form definition.</param>
        /// <returns>The save event table name, or null when there is no resolvable save event.</returns>
        private async Task<string> GetSaveEventTableNameAsync(FormConfig form)
        {
            if (string.IsNullOrWhiteSpace(form?.SaveEvent))
            {
                return null;
            }

            try
            {
                var saveEvent = await _eventAdapter.GetEventAsync(form.SaveEvent);
                return saveEvent?.TableName;
            }
            catch (Exception ex)
            {
                _logWriter?.LogInfo(
                    $"Save event {form.SaveEvent} for form {form.Name} could not be resolved during scope resolution: {ex.Message}",
                    ClassName,
                    nameof(GetSaveEventTableNameAsync));
                return null;
            }
        }

        /// <summary>
        /// Enumerates every non-null, identified field on a page across all columns and form groups.
        /// </summary>
        /// <param name="page">Page definition.</param>
        /// <returns>The page's fields.</returns>
        private static IEnumerable<FieldConfig> EnumerateFields(PageConfig page)
        {
            return (page.Columns ?? [])
                .SelectMany(c => c?.FormGroups ?? [])
                .SelectMany(fg => fg?.Fields ?? [])
                .Where(f => f != null && !string.IsNullOrWhiteSpace(f.Id));
        }

        /// <summary>
        /// Logs a single excluded candidate so an empty or unexpected picker can be diagnosed from a
        /// production log.
        /// </summary>
        /// <param name="fieldId">Excluded field id.</param>
        /// <param name="reason">Exclusion reason.</param>
        private void LogExclusion(string fieldId, string reason)
        {
            _logWriter?.LogInfo(
                $"Existing field excluded: field={fieldId}, reason={reason}",
                ClassName,
                nameof(BuildAsync));
        }
    }
}
