using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Executes the <c>addexistingfield</c> configuration event, which places fields that already
    /// exist on other forms onto a target page by reference.
    /// </summary>
    /// <remarks>
    /// Each selected reference is a composite id of <c>{sourceForm}|{sourcePage}|{fieldId}</c>. The
    /// field configuration is cloned onto the target page keeping its id verbatim, so the referenced
    /// field resolves to the same physical column or MoreData key as the original and the two forms
    /// read and write the same data. No new name is reserved in <c>namelist</c>.
    /// </remarks>
    internal class AddExistingFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        private const string ClassName = nameof(AddExistingFieldEvent);

        /// <summary>
        /// Creates a new <see cref="AddExistingFieldEvent"/>.
        /// </summary>
        /// <param name="serviceProvider">Service provider for required configuration services.</param>
        public AddExistingFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Adds each selected existing field to the target form group.
        /// </summary>
        /// <param name="dataToSave">JSON payload matching <see cref="AddExistingFieldModel"/>.</param>
        /// <param name="id">
        /// Target context of <c>{formName}|{pageName}|{columnKey}|{formGroupKey}|{viewConfigId}</c>.
        /// Only the form and page names are required.
        /// </param>
        /// <param name="command">The event command metadata.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>0 on success.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var payload = JsonConvert.DeserializeObject<AddExistingFieldModel>(dataToSave, settings);
            var idSource = !string.IsNullOrWhiteSpace(payload?.Id) ? payload.Id : id;
            var idList = (idSource ?? string.Empty).Split("|");

            if (idList.Length < 2 || string.IsNullOrWhiteSpace(idList[0]) || string.IsNullOrWhiteSpace(idList[1]))
            {
                logWriter?.LogError(
                    $"AddExistingField failed: malformed id '{idSource}', expected form|page[|column|formGroup|viewId]",
                    ClassName,
                    nameof(RunAsync));
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];
            var columnKey = idList.Length > 2 ? NullIfEmpty(idList[2]) : null;
            var formGroupKey = idList.Length > 3 ? NullIfEmpty(idList[3]) : null;

            var referenceKeys = SplitReferenceKeys(payload?.ExistingFieldIds);

            logWriter?.LogInfo(
                $"AddExistingField: form={formName}, page={pageName}, formGroup={formGroupKey ?? "last"}, requested={referenceKeys.Count} references",
                ClassName,
                nameof(RunAsync));

            if (referenceKeys.Count == 0)
            {
                return 0;
            }

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var fieldConfigUtils = _serviceProvider.GetService<IFieldConfigUtils>();
            var catalogue = _serviceProvider.GetService<IExistingFieldCatalogue>();

            var formToUpdate = await formConfigDefinition.LoadFormAsync(formName);
            if (formToUpdate == null)
            {
                logWriter?.LogError(
                    $"AddExistingField failed: form={formName} could not be loaded",
                    ClassName,
                    nameof(RunAsync));
                return 0;
            }

            var targetPage = formToUpdate.GetPage(pageName);
            if (targetPage == null)
            {
                logWriter?.LogError(
                    $"AddExistingField failed: page={pageName} is not part of form={formName}",
                    ClassName,
                    nameof(RunAsync));
                return 0;
            }

            var targetScope = FieldReuseScopeExtensions.ResolveScope(
                targetPage.TableName,
                formToUpdate.SingleItemName,
                formToUpdate.SaveEventConfig?.TableName);

            var added = 0;
            var skipped = 0;
            var idsInThisBatch = referenceKeys
                .Select(k => FieldReuseScopeExtensions.ParseFieldReferenceKey(k).FieldId)
                .Where(f => f != null)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var referenceKey in referenceKeys)
            {
                try
                {
                    var skipReason = await AddOneReferenceAsync(
                        referenceKey,
                        formToUpdate,
                        pageName,
                        columnKey,
                        formGroupKey,
                        targetScope,
                        idsInThisBatch,
                        catalogue,
                        formConfigDefinition,
                        fieldConfigUtils,
                        logWriter);

                    if (skipReason == null)
                    {
                        added++;
                    }
                    else
                    {
                        skipped++;
                        logWriter?.LogInfo(
                            $"AddExistingField: skipped field={referenceKey}, reason={skipReason}",
                            ClassName,
                            nameof(RunAsync));
                    }
                }
                catch (Exception ex)
                {
                    skipped++;
                    logWriter?.LogError(
                        $"AddExistingField failed: form={formName}, page={pageName}, key={referenceKey}, error={ex.Message}",
                        ClassName,
                        nameof(RunAsync));
                }
            }

            if (added > 0)
            {
                await formConfigRepository.UpdateFormAsync(formToUpdate);
            }

            logWriter?.LogInfo(
                $"AddExistingField complete: form={formName}, added={added}, skipped={skipped}",
                ClassName,
                nameof(RunAsync));

            return 0;
        }

        /// <summary>
        /// Resolves and places a single field reference on the target page.
        /// </summary>
        /// <param name="referenceKey">Reference key <c>{sourceForm}|{sourcePage}|{fieldId}</c>.</param>
        /// <param name="formToUpdate">Target form being mutated.</param>
        /// <param name="pageName">Target page name id.</param>
        /// <param name="columnKey">Target column key, or null.</param>
        /// <param name="formGroupKey">Target form group key, or null.</param>
        /// <param name="targetScope">Resolved target entity table, or null when unscoped.</param>
        /// <param name="idsInThisBatch">Field ids selected in the same submission, used for parent link checks.</param>
        /// <param name="catalogue">Catalogue used to resolve the reference.</param>
        /// <param name="formConfigDefinition">Loader for the source form.</param>
        /// <param name="fieldConfigUtils">Utility that performs the placement and propagation.</param>
        /// <param name="logWriter">Structured log writer.</param>
        /// <returns>Null when the field was added; otherwise the reason it was skipped.</returns>
        private static async Task<string> AddOneReferenceAsync(
            string referenceKey,
            FullFormConfig formToUpdate,
            string pageName,
            string columnKey,
            string formGroupKey,
            string targetScope,
            HashSet<string> idsInThisBatch,
            IExistingFieldCatalogue catalogue,
            IFormConfigDefinition formConfigDefinition,
            IFieldConfigUtils fieldConfigUtils,
            ILogWriter logWriter)
        {
            var candidate = await catalogue.ResolveAsync(referenceKey);
            if (candidate == null)
            {
                return "missingSource";
            }

            if (!FieldReuseScopeExtensions.ScopesMatch(targetScope, candidate.TableName))
            {
                return $"scopeMismatch (target={targetScope}, candidate={candidate.TableName ?? "none"})";
            }

            // Per page, not per form: the same definition may appear on a second page of the form as
            // a read only reference copy.
            var existingIds = formToUpdate.GetPage(pageName)?
                .GetFieldList()
                .Where(f => f?.Id != null)
                .Select(f => f.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (existingIds.Contains(candidate.FieldId))
            {
                return "alreadyOnPage";
            }

            var sourceForm = await formConfigDefinition.LoadFormAsync(candidate.SourceForm);
            var sourcePage = sourceForm?.GetPage(candidate.SourcePage);
            var sourceField = sourcePage?
                .GetFieldList()
                .FirstOrDefault(f => f.Id.Equals(candidate.FieldId, StringComparison.OrdinalIgnoreCase));

            if (sourceField == null)
            {
                return "missingSourceField";
            }

            var requiredErrorMessage = sourceField.Required
                ? sourceForm.SaveEventConfig?.GetValidationRuleForField(sourceField.Id)?.Message
                : null;

            var parentListDropped = false;
            if (!string.IsNullOrWhiteSpace(sourceField.ParentList)
                && !existingIds.Contains(sourceField.ParentList)
                && !idsInThisBatch.Contains(sourceField.ParentList))
            {
                logWriter?.LogInfo(
                    $"AddExistingField: ParentList '{sourceField.ParentList}' dropped for field={sourceField.Id} - parent field not present on target form {formToUpdate.Name}",
                    ClassName,
                    nameof(AddOneReferenceAsync));
                parentListDropped = true;
            }

            logWriter?.LogInfo(
                $"AddExistingField: referencing field={sourceField.Id}, type={sourceField.Type}, from form={candidate.SourceForm} page={candidate.SourcePage}, targetTable={targetScope ?? "none"}, parentListDropped={parentListDropped}",
                ClassName,
                nameof(AddOneReferenceAsync));

            var fieldToPlace = sourceField;
            if (parentListDropped)
            {
                fieldToPlace = JsonConvert.DeserializeObject<FieldConfig>(JsonConvert.SerializeObject(sourceField));
                fieldToPlace.ParentList = null;
            }

            await fieldConfigUtils.AddExistingFieldToSystemAsync(
                fieldToPlace,
                formToUpdate,
                pageName,
                requiredErrorMessage,
                formGroupKey,
                columnKey);

            return null;
        }

        /// <summary>
        /// Splits the comma separated reference key list, trimming blanks and duplicates.
        /// </summary>
        /// <param name="existingFieldIds">Raw comma separated reference keys from the payload.</param>
        /// <returns>The distinct reference keys in submission order.</returns>
        private static List<string> SplitReferenceKeys(string existingFieldIds)
        {
            if (string.IsNullOrWhiteSpace(existingFieldIds))
            {
                return [];
            }

            return existingFieldIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// Returns null for an empty or whitespace id segment so optional parts are easy to test.
        /// </summary>
        /// <param name="value">Raw id segment.</param>
        /// <returns>The trimmed value, or null when blank.</returns>
        private static string NullIfEmpty(string value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
