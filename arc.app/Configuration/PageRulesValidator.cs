using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates an editpagerules payload before it is applied. Protects system owned state
    /// definitions from being changed, keeps state names usable by the runtime state matcher, and
    /// makes sure a page can only be gated by a state that is reachable before it.
    /// </summary>
    internal class PageRulesValidator : ISpecialValidatorAsync
    {
        private static readonly string[] SupportedRules = { "=", "!=", "isempty", "isnotempty" };

        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly ILogWriter _logWriter;
        private readonly string _message;

        internal PageRulesValidator(IFormConfigDefinition formConfigDefinition, string message, ILogWriter logWriter)
        {
            _formConfigDefinition = formConfigDefinition;
            _message = message;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Validates the payload and returns the first problem found.
        /// </summary>
        /// <returns>An empty string when the payload is acceptable, otherwise a translation tag.</returns>
        public async Task<string> ValidateMessageAsync()
        {
            var model = PageRulesEventPayloadReader.Read(_message, _logWriter);
            var idList = (model.Id ?? "").Split('|');
            if (idList.Length < 2)
            {
                _logWriter?.LogInfo($"PageRulesValidator: unusable id '{model.Id}'", nameof(PageRulesValidator), nameof(ValidateMessageAsync));
                return "";
            }

            var formName = idList[0];
            var pageName = idList[1];

            var form = await _formConfigDefinition.LoadFormAsync(formName);
            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name.IsSameAs(pageName));
            if (page == null)
            {
                _logWriter?.LogInfo(
                    $"PageRulesValidator: page {pageName} was not found on form {formName}",
                    nameof(PageRulesValidator),
                    nameof(ValidateMessageAsync));
                return "";
            }

            var detail = model.PageRules ?? new PageRuleDetailModel();

            var lockMessage = ValidateLocks(page, detail);
            if (!string.IsNullOrEmpty(lockMessage)) return Reject(lockMessage, formName, pageName, -1, null);

            var stateMessage = ValidateStateRules(form, page, formName, pageName, detail);
            if (!string.IsNullOrEmpty(stateMessage)) return stateMessage;

            var visibilityMessage = ValidateVisibility(form, pageName, detail);
            if (!string.IsNullOrEmpty(visibilityMessage)) return Reject(visibilityMessage, formName, pageName, -1, null);

            return "";
        }

        /// <summary>
        /// Rejects a payload, recording the reason so the decision can be traced on an installed system.
        /// </summary>
        /// <param name="message">The translation tag describing the problem.</param>
        /// <param name="formName">The form being configured.</param>
        /// <param name="pageName">The page being configured.</param>
        /// <param name="ruleIndex">Zero-based index of the failing rule, or -1 when not rule specific.</param>
        /// <param name="effect">The effect token for the failing rule, if applicable.</param>
        /// <returns>The supplied message.</returns>
        private string Reject(string message, string formName, string pageName, int ruleIndex, string effect)
        {
            var ruleDetail = ruleIndex >= 0
                ? $", ruleIndex={ruleIndex}, effect='{effect ?? ""}'"
                : "";

            _logWriter?.LogInfo(
                $"PageRulesValidator: rejected change to {formName}|{pageName} with {message}{ruleDetail}",
                nameof(PageRulesValidator),
                nameof(ValidateMessageAsync));
            return message;
        }

        /// <summary>
        /// Refuses any change to a state definition that the system owns. The visibility rule is not
        /// guarded here because gating a page on a state does not alter how that state is defined.
        /// </summary>
        /// <param name="page">The page being configured.</param>
        /// <param name="detail">The submitted values.</param>
        /// <returns>A translation tag when a locked value was changed, otherwise an empty string.</returns>
        private string ValidateLocks(PageConfig page, PageRuleDetailModel detail)
        {
            if (!PageStateUtils.IsPageStateLocked(page)) return "";

            var submittedRules = PageRulesEventPayloadReader.GetEffectiveStateRules(detail)
                .Where(r => !string.IsNullOrEmpty(r.Effect))
                .ToList();

            var existingRules = (page.NextButton?.OnClickState?.Rules ?? new List<RuleConfig>())
                .Where(r => r != null && !string.IsNullOrWhiteSpace(r.Effect))
                .Select(r => new PageRuleStateRuleModel
                {
                    Effect = r.Effect,
                    Field = r.Field ?? "",
                    Rule = string.IsNullOrWhiteSpace(r.Rule) ? "=" : r.Rule,
                    Value = r.Value ?? ""
                })
                .ToList();

            if (submittedRules.Count != existingRules.Count) return "@ConPagLocB@";

            for (var index = 0; index < existingRules.Count; index++)
            {
                var existing = existingRules[index];
                var submitted = submittedRules[index];

                if (submitted.Effect.IsNotSameAs(existing.Effect)
                    || submitted.Field.IsNotSameAs(existing.Field)
                    || submitted.Rule.IsNotSameAs(existing.Rule)
                    || submitted.Value.IsNotSameAs(existing.Value))
                {
                    return "@ConPagLocB@";
                }
            }

            return "";
        }

        /// <summary>
        /// Validates the state rules the page sets, including effect names and each condition row.
        /// </summary>
        /// <param name="form">The form being configured.</param>
        /// <param name="page">The page being configured.</param>
        /// <param name="formName">The form name as supplied in the payload id.</param>
        /// <param name="pageName">The page name as supplied in the payload.</param>
        /// <param name="detail">The submitted values.</param>
        /// <returns>A translation tag when the state is not acceptable, otherwise an empty string.</returns>
        private string ValidateStateRules(FullFormConfig form, PageConfig page, string formName, string pageName, PageRuleDetailModel detail)
        {
            var stateRules = PageRulesEventPayloadReader.GetEffectiveStateRules(detail)
                .Where(r => !string.IsNullOrEmpty(r.Effect))
                .ToList();

            var declaredEffects = PageStateUtils.GetDeclaredEffectsForPage(page);

            if (stateRules.Count == 0)
            {
                if (declaredEffects.Count == 0) return "";

                foreach (var effect in declaredEffects)
                {
                    var dependants = (form.Rules ?? new List<FormRulesConfig>())
                        .Where(r => r?.State.IsSameAs(effect) == true)
                        .ToList();

                    if (dependants.Any())
                    {
                        return Reject("@ConStaUse@", formName, pageName, -1, effect);
                    }
                }

                return "";
            }

            var existingEffects = declaredEffects.ToList();
            var validatedEffects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (var index = 0; index < stateRules.Count; index++)
            {
                var rule = stateRules[index];

                if (!validatedEffects.Add(rule.Effect))
                {
                    // Name validation already performed for this effect.
                }
                else
                {
                    var isNewEffect = !existingEffects.Any(e => e.IsSameAs(rule.Effect));
                    if (isNewEffect)
                    {
                        var nameMessage = PageStateUtils.ValidateStateName(form, pageName, rule.Effect);
                        if (!string.IsNullOrEmpty(nameMessage))
                        {
                            return Reject(nameMessage, formName, pageName, index, rule.Effect);
                        }
                    }
                }

                if (string.IsNullOrEmpty(rule.Field))
                {
                    return Reject("@ConStaFie@", formName, pageName, index, rule.Effect);
                }

                if (!page.GetFieldList().Any(f => f?.Id.IsSameAs(rule.Field) == true))
                {
                    return Reject("@ConStaFieA@", formName, pageName, index, rule.Effect);
                }

                if (!SupportedRules.Any(r => r.IsSameAs(rule.Rule)))
                {
                    return Reject("@ConStaRul@", formName, pageName, index, rule.Effect);
                }

                var needsValue = rule.Rule.IsSameAs("=") || rule.Rule.IsSameAs("!=");
                if (needsValue && string.IsNullOrEmpty(rule.Value))
                {
                    return Reject("@ConStaVal@", formName, pageName, index, rule.Effect);
                }
            }

            return "";
        }

        /// <summary>
        /// Validates the state chosen to control the visibility of the page.
        /// </summary>
        /// <param name="form">The form being configured.</param>
        /// <param name="pageName">The page name as supplied in the payload.</param>
        /// <param name="detail">The submitted values.</param>
        /// <returns>A translation tag when the state cannot gate the page, otherwise an empty string.</returns>
        private static string ValidateVisibility(FullFormConfig form, string pageName, PageRuleDetailModel detail)
        {
            if (string.IsNullOrEmpty(detail.VisibleWhenState)) return "";

            var declaredEffects = PageRulesEventPayloadReader.GetEffectiveStateRules(detail)
                .Select(r => r.Effect)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (declaredEffects.Any(e => e.IsSameAs(detail.VisibleWhenState))) return "@ConStaSelA@";

            var existingRule = PageStateUtils.GetVisibilityRuleForPage(form, pageName);
            if (existingRule != null && detail.VisibleWhenState.IsSameAs(existingRule.State)) return "";

            var selectable = PageStateUtils.GetSelectableStatesForPage(form, pageName);
            return selectable.Any(s => s.State.IsSameAs(detail.VisibleWhenState)) ? "" : "@ConStaSel@";
        }
    }
}
