using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Saves the visibility rule and the state definition for a single page.
    /// The visibility rule is stored on the form as a { visible, page, state } entry; the state the
    /// page sets is stored on the page next button as an on click state marked configurable.
    /// A change to a system owned state definition is rejected by <see cref="PageRulesValidator"/>
    /// before this runs and is guarded again here in case the event is invoked directly. The
    /// visibility rule is not guarded, because any state declared by an earlier page may gate a page.
    /// </summary>
    internal class EditPageRulesEvent : IRun
    {
        private const string ConfigurableFlag = "Yes";
        private const string NextButtonTextTag = "@GenNex@";

        private readonly IServiceProvider _serviceProvider;

        public EditPageRulesEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Applies the submitted page rules to the form and persists the whole form configuration.
        /// </summary>
        /// <param name="dataToSave">Raw event JSON from the client.</param>
        /// <param name="id">Fallback identifier when the payload does not carry one.</param>
        /// <param name="command">The event command wrapper.</param>
        /// <param name="eventData">The event definition. Not used by this event.</param>
        /// <returns>Zero; this event does not create a record.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var dataModel = PageRulesEventPayloadReader.Read(dataToSave, logWriter);

            var idSource = !string.IsNullOrEmpty(dataModel.Id) ? dataModel.Id : id;
            var idList = (idSource ?? "").Split('|');
            if (idList.Length < 2)
            {
                logWriter?.LogInfo($"EditPageRulesEvent: unusable id '{idSource}'", nameof(EditPageRulesEvent), nameof(RunAsync));
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];
            var detail = dataModel.PageRules ?? new PageRuleDetailModel();

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name.IsSameAs(pageName));
            if (page == null)
            {
                logWriter?.LogInfo($"EditPageRulesEvent: page {pageName} was not found on form {formName}", nameof(EditPageRulesEvent), nameof(RunAsync));
                return 0;
            }

            if (PageStateUtils.IsPageStateLocked(page))
            {
                logWriter?.LogInfo(
                    $"EditPageRulesEvent: the state set by {formName}|{pageName} is system owned, only the visibility rule will be considered",
                    nameof(EditPageRulesEvent),
                    nameof(RunAsync));
            }
            else
            {
                ApplyStateDefinition(form, page, detail, logWriter);
            }

            ApplyVisibilityRule(form, page, detail, logWriter);

            await formConfigRepository.UpdateFormAsync(form);

            logWriter?.LogInfo(
                $"EditPageRulesEvent: saved {formName}|{pageName} with {form.Rules?.Count ?? 0} form rule(s)",
                nameof(EditPageRulesEvent),
                nameof(RunAsync));

            return 0;
        }

        /// <summary>
        /// Writes the state rules the page applies when the user moves on. Clearing all rules removes
        /// the on click state; renaming an effect cascades the new name across form visibility rules.
        /// </summary>
        /// <param name="form">The form being updated.</param>
        /// <param name="page">The page being updated.</param>
        /// <param name="detail">The submitted values.</param>
        /// <param name="logWriter">Log writer used to record what changed.</param>
        private static void ApplyStateDefinition(FullFormConfig form, PageConfig page, PageRuleDetailModel detail, ILogWriter logWriter)
        {
            var stateRules = PageRulesEventPayloadReader.GetEffectiveStateRules(detail)
                .Where(r => !string.IsNullOrEmpty(r.Effect))
                .ToList();

            var oldEffects = PageStateUtils.GetDeclaredEffectsForPage(page);

            if (stateRules.Count == 0)
            {
                if (page.NextButton?.OnClickState == null) return;

                page.NextButton.OnClickState = null;
                logWriter?.LogInfo(
                    $"EditPageRulesEvent: removed state definition from page {page.Name}",
                    nameof(EditPageRulesEvent),
                    nameof(ApplyStateDefinition));
                return;
            }

            if (page.NextButton == null)
            {
                page.NextButton = new NextButtonConfig { ButtonText = NextButtonTextTag, Show = true };
                logWriter?.LogInfo(
                    $"EditPageRulesEvent: created a next button on page {page.Name} so it can set a state",
                    nameof(EditPageRulesEvent),
                    nameof(ApplyStateDefinition));
            }

            var ruleConfigs = stateRules.Select(r =>
            {
                var needsValue = r.Rule.IsSameAs("=") || r.Rule.IsSameAs("!=");
                return new RuleConfig
                {
                    Effect = r.Effect,
                    Field = r.Field,
                    Rule = r.Rule,
                    Value = needsValue ? r.Value : ""
                };
            }).ToList();

            var primaryState = stateRules[0].Effect;
            var newEffects = stateRules
                .Select(r => r.Effect)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            page.NextButton.OnClickState = new NextButtonClickConfig
            {
                State = primaryState,
                Configurable = ConfigurableFlag,
                Rules = ruleConfigs
            };

            logWriter?.LogInfo(
                $"EditPageRulesEvent: page {page.Name} now sets {ruleConfigs.Count} rule(s) across {newEffects.Count} effect(s), primary state '{primaryState}'",
                nameof(EditPageRulesEvent),
                nameof(ApplyStateDefinition));

            CascadeEffectRenames(form, oldEffects, newEffects, logWriter);
        }

        /// <summary>
        /// Renames visibility rules when a declared effect was renamed rather than removed.
        /// </summary>
        /// <param name="form">The form being updated.</param>
        /// <param name="oldEffects">Effects declared before the save.</param>
        /// <param name="newEffects">Effects declared after the save.</param>
        /// <param name="logWriter">Log writer used to record cascaded renames.</param>
        private static void CascadeEffectRenames(FullFormConfig form, List<string> oldEffects, List<string> newEffects, ILogWriter logWriter)
        {
            var removedEffects = oldEffects
                .Where(o => !newEffects.Any(n => n.IsSameAs(o)))
                .ToList();
            var addedEffects = newEffects
                .Where(n => !oldEffects.Any(o => o.IsSameAs(n)))
                .ToList();

            if (removedEffects.Count != 1 || addedEffects.Count != 1) return;

            var oldEffect = removedEffects[0];
            var newEffect = addedEffects[0];
            var renamed = 0;

            foreach (var rule in form.Rules ?? new List<FormRulesConfig>())
            {
                if (rule?.State.IsSameAs(oldEffect) != true) continue;
                rule.State = newEffect;
                renamed++;
            }

            if (renamed > 0)
            {
                logWriter?.LogInfo(
                    $"EditPageRulesEvent: renamed state '{oldEffect}' to '{newEffect}' on {renamed} visibility rule(s)",
                    nameof(EditPageRulesEvent),
                    nameof(CascadeEffectRenames));
            }
        }

        /// <summary>
        /// Writes the visibility rule for the page. An empty state makes the page always visible and
        /// removes any existing rule.
        /// </summary>
        /// <param name="form">The form being updated.</param>
        /// <param name="page">The page being updated.</param>
        /// <param name="detail">The submitted values.</param>
        /// <param name="logWriter">Log writer used to record what changed.</param>
        private static void ApplyVisibilityRule(FullFormConfig form, PageConfig page, PageRuleDetailModel detail, ILogWriter logWriter)
        {
            form.Rules ??= new List<FormRulesConfig>();

            var existingRule = PageStateUtils.GetVisibilityRuleForPage(form, page.Name);
            var oldState = existingRule?.State ?? "";

            if (existingRule != null)
            {
                form.Rules.Remove(existingRule);
            }

            if (!string.IsNullOrEmpty(detail.VisibleWhenState))
            {
                form.Rules.Add(new FormRulesConfig
                {
                    Outcome = PageStateUtils.VisibleOutcome,
                    Page = page.Name,
                    State = detail.VisibleWhenState
                });
            }

            logWriter?.LogInfo(
                $"EditPageRulesEvent: visibility of page {page.Name} changed from '{oldState}' to '{detail.VisibleWhenState}'",
                nameof(EditPageRulesEvent),
                nameof(ApplyVisibilityRule));
        }
    }
}
