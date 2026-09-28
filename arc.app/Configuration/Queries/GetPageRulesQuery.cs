using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Retrieves the visibility rule and state definition for a single page, identified by
    /// formName|pageName. Used when opening the page visibility configuration form.
    /// </summary>
    internal class GetPageRulesQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetPageRulesQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the page rules as JSON containing the current settings, the states the user may
        /// choose from, the fields available for the state condition and the read only flag that
        /// protects a system owned state definition.
        /// </summary>
        /// <param name="queryFilters">Query filters carrying the "id" parameter as formName|pageName.</param>
        /// <param name="queryData">The query definition. Not used by this query.</param>
        /// <returns>A JSON string describing the page rules.</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");

            var logWriter = _serviceProvider.GetService<ILogWriter>();

            if (idList.Length < 2)
            {
                logWriter?.LogInfo($"GetPageRulesQuery: unusable id '{id}'", nameof(GetPageRulesQuery), nameof(GetAsync));
                return JsonConvert.SerializeObject(BuildEmptyResult(id));
            }

            var formName = idList[0];
            var pageName = idList[1];

            logWriter?.LogInfo($"GetPageRulesQuery: formName={formName}, pageName={pageName}", nameof(GetPageRulesQuery), nameof(GetAsync));

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name.IsSameAs(pageName));

            if (page == null)
            {
                logWriter?.LogInfo($"GetPageRulesQuery: page {pageName} was not found on form {formName}", nameof(GetPageRulesQuery), nameof(GetAsync));
                return JsonConvert.SerializeObject(BuildEmptyResult(id));
            }

            var existingRule = PageStateUtils.GetVisibilityRuleForPage(form, pageName);
            var onClickState = page.NextButton?.OnClickState;
            var stateRules = GetStateRules(onClickState);
            var stateLocked = PageStateUtils.IsPageStateLocked(page);

            var selectableStates = PageStateUtils.GetSelectableStatesForPage(form, pageName)
                .Select(s => new { id = s.State, label = s.State, page = s.DeclaredByPage, pageTitle = s.DeclaredByPageTitle })
                .ToList();

            var fieldOptions = BuildFieldOptionsForPage(page);

            var firstRule = stateRules.FirstOrDefault();
            var pageRules = new PageRuleDetailModel
            {
                VisibleWhenState = existingRule?.State ?? "",
                StateRules = stateRules,
                SetsState = firstRule?.Effect ?? onClickState?.State ?? "",
                ConditionField = firstRule?.Field ?? "",
                ConditionRule = string.IsNullOrWhiteSpace(firstRule?.Rule) ? "=" : firstRule.Rule,
                ConditionValue = firstRule?.Value ?? ""
            };

            var effectCount = stateRules
                .Select(r => r.Effect ?? "")
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            logWriter?.LogInfo(
                $"GetPageRulesQuery: visibleWhenState='{pageRules.VisibleWhenState}', ruleCount={stateRules.Count}, " +
                $"effectCount={effectCount}, stateLocked={stateLocked}, selectableStates=[{string.Join(", ", selectableStates.Select(s => s.id))}], " +
                $"fieldOptions={fieldOptions.Count}",
                nameof(GetPageRulesQuery),
                nameof(GetAsync));

            var result = new
            {
                Id = id,
                PageRules = pageRules,
                stateOptions = selectableStates,
                fieldOptions,
                stateLocked
            };

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Builds the payload returned when the form or page could not be resolved, so the editor
        /// still renders rather than failing.
        /// </summary>
        /// <param name="id">The identifier that was requested.</param>
        /// <returns>An empty page rules payload.</returns>
        private static object BuildEmptyResult(string id)
        {
            return new
            {
                Id = id,
                PageRules = new PageRuleDetailModel
                {
                    VisibleWhenState = "",
                    StateRules = new List<PageRuleStateRuleModel>(),
                    SetsState = "",
                    ConditionField = "",
                    ConditionRule = "=",
                    ConditionValue = ""
                },
                stateOptions = new List<object>(),
                fieldOptions = new List<object>(),
                stateLocked = true
            };
        }

        /// <summary>
        /// Maps on click state rules to page rule state models for the editor.
        /// </summary>
        /// <param name="onClickState">The on click state to inspect. May be null.</param>
        /// <returns>All state rules for the editor.</returns>
        private static List<PageRuleStateRuleModel> GetStateRules(NextButtonClickConfig onClickState)
        {
            if (onClickState?.Rules == null || onClickState.Rules.Count == 0)
            {
                if (string.IsNullOrWhiteSpace(onClickState?.State)) return new List<PageRuleStateRuleModel>();

                return new List<PageRuleStateRuleModel>
                {
                    new PageRuleStateRuleModel
                    {
                        Effect = onClickState.State,
                        Field = "",
                        Rule = "=",
                        Value = ""
                    }
                };
            }

            return onClickState.Rules
                .Where(r => r != null && !string.IsNullOrWhiteSpace(r.Effect))
                .Select(r => new PageRuleStateRuleModel
                {
                    Effect = r.Effect,
                    Field = r.Field ?? "",
                    Rule = string.IsNullOrWhiteSpace(r.Rule) ? "=" : r.Rule,
                    Value = r.Value ?? ""
                })
                .ToList();
        }

        /// <summary>
        /// Builds the list of fields on the page that can drive a state condition. Upload and field
        /// grid fields are excluded because they cannot be compared with a simple rule.
        /// </summary>
        /// <param name="page">The page whose fields are offered.</param>
        /// <returns>Field options carrying id, label, type and options list name.</returns>
        private static List<object> BuildFieldOptionsForPage(PageConfig page)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var fieldOptions = new List<object>();

            foreach (var field in page.GetFieldList() ?? new List<FieldConfig>())
            {
                if (string.IsNullOrEmpty(field.Id) || !seen.Add(field.Id)) continue;
                if (field.Type.IsSameAs("upload") || field.Type.IsSameAs("fieldgrid")) continue;

                var type = field.Type ?? "";
                var isListBacked = type.IsSameAs("dropdown") || type.IsSameAs("combobox") || type.IsSameAs("radio")
                                   || type.IsSameAs("picker") || type.IsSameAs("hierarchicalpicker");

                fieldOptions.Add(new
                {
                    id = field.Id,
                    label = string.IsNullOrEmpty(field.Label) ? field.Id : field.Label,
                    type,
                    optionsName = isListBacked ? field.OptionsName ?? "" : ""
                });
            }

            return fieldOptions;
        }
    }
}
