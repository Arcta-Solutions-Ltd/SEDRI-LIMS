using arc.common.ExtensionMethods;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace arc.app.Configuration
{
    /// <summary>
    /// Describes a workflow state declared by a page of a form.
    /// </summary>
    internal class PageStateOption
    {
        /// <summary>
        /// Gets or sets the state token, for example "bloodspecimen".
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the name of the page whose next button declares the state.
        /// </summary>
        public string DeclaredByPage { get; set; }

        /// <summary>
        /// Gets or sets the title of the page whose next button declares the state.
        /// </summary>
        public string DeclaredByPageTitle { get; set; }

        /// <summary>
        /// Gets or sets whether the state was marked configurable, meaning a user may change it.
        /// </summary>
        public bool Configurable { get; set; }

        /// <summary>
        /// Gets or sets the zero based position of the declaring page within the form.
        /// </summary>
        public int PageIndex { get; set; }
    }

    /// <summary>
    /// Discovers and validates the workflow states declared by the pages of a form.
    /// States are only ever introduced by a form start state, a page entry state, or a page next
    /// button on click state, and only on click states marked configurable may be edited by a user.
    /// </summary>
    internal static class PageStateUtils
    {
        /// <summary>
        /// The only outcome currently supported by form level page rules.
        /// </summary>
        internal const string VisibleOutcome = "visible";

        private static readonly Regex StateNamePattern = new Regex("^[a-z][a-z0-9]*$", RegexOptions.Compiled);

        private const int MaximumStateNameLength = 30;

        /// <summary>
        /// Returns every state declared by a next button on click state on the pages of the form,
        /// in page order. Each distinct effect in the on click rules is returned as a separate option.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <returns>The declared states; an empty list when the form has no resolved pages.</returns>
        internal static List<PageStateOption> GetStatesForForm(FullFormConfig form)
        {
            var states = new List<PageStateOption>();
            if (form?.PagesConfig == null) return states;

            for (var index = 0; index < form.PagesConfig.Count; index++)
            {
                var page = form.PagesConfig[index];
                var onClickState = page?.NextButton?.OnClickState;
                if (onClickState == null) continue;

                var effects = GetDistinctEffectsFromOnClickState(onClickState);
                if (effects.Count == 0) continue;

                var configurable = onClickState.Configurable.IsSameAs("Yes");
                foreach (var effect in effects)
                {
                    states.Add(new PageStateOption
                    {
                        State = effect,
                        DeclaredByPage = page.Name,
                        DeclaredByPageTitle = page.PageTitle,
                        Configurable = configurable,
                        PageIndex = index
                    });
                }
            }

            return states;
        }

        /// <summary>
        /// Returns the distinct state effects declared by a page's on click state.
        /// </summary>
        /// <param name="page">The page to inspect.</param>
        /// <returns>Distinct effect tokens declared by the page.</returns>
        internal static List<string> GetDeclaredEffectsForPage(PageConfig page)
        {
            var onClickState = page?.NextButton?.OnClickState;
            if (onClickState == null) return new List<string>();

            return GetDistinctEffectsFromOnClickState(onClickState);
        }

        /// <summary>
        /// Returns every state token that can ever appear in the running form state, including the
        /// form start state and any page entry states as well as the declared on click states.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <returns>The distinct state tokens, lower cased.</returns>
        internal static List<string> GetAllStateTokens(FullFormConfig form)
        {
            var tokens = new List<string>();
            if (form == null) return tokens;

            AddTokens(tokens, form.StartState);

            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                AddTokens(tokens, page?.EntryState);
                AddTokens(tokens, page?.NextButton?.OnClickState?.State);
                foreach (var effect in GetDistinctEffectsFromOnClickState(page?.NextButton?.OnClickState))
                {
                    AddTokens(tokens, effect);
                }
                AddTokens(tokens, page?.PrevButton?.OnClickState?.State);
                AddTokens(tokens, page?.CancelButton?.OnClickState?.State);
                AddTokens(tokens, page?.NextItemButton?.OnClickState?.State);
            }

            foreach (var rule in form.Rules ?? new List<FormRulesConfig>())
            {
                AddTokens(tokens, rule?.State);
            }

            return tokens.Distinct().ToList();
        }

        /// <summary>
        /// Returns the states a user may pick to control the visibility of the supplied page. Any
        /// state declared by a page that appears earlier in the form may be used, including system
        /// owned states, because gating a page on a state does not change how that state is defined.
        /// A state declared later is not offered because it can never be active when the page is
        /// reached.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <param name="pageName">The page whose visibility is being configured.</param>
        /// <returns>The selectable states in page order.</returns>
        internal static List<PageStateOption> GetSelectableStatesForPage(FullFormConfig form, string pageName)
        {
            var pageIndex = GetPageIndex(form, pageName);
            if (pageIndex < 0) return new List<PageStateOption>();

            return GetStatesForForm(form)
                .Where(s => s.PageIndex < pageIndex)
                .Where(s => s.DeclaredByPage.IsNotSameAs(pageName))
                .ToList();
        }

        /// <summary>
        /// Determines whether the state declared by a page is owned by the system and so cannot be
        /// changed. A page that declares no state is not locked because a user may add one.
        /// </summary>
        /// <param name="page">The page to inspect.</param>
        /// <returns>True when the declared state must be left alone.</returns>
        internal static bool IsPageStateLocked(PageConfig page)
        {
            var onClickState = page?.NextButton?.OnClickState;
            if (string.IsNullOrWhiteSpace(onClickState?.State)) return false;

            return !onClickState.Configurable.IsSameAs("Yes");
        }

        /// <summary>
        /// Returns the visibility rule that currently applies to a page, or null when the page is
        /// always visible.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <param name="pageName">The page name.</param>
        /// <returns>The matching rule or null.</returns>
        internal static FormRulesConfig GetVisibilityRuleForPage(FullFormConfig form, string pageName)
        {
            return (form?.Rules ?? new List<FormRulesConfig>())
                .FirstOrDefault(r => r != null
                                     && r.Page.IsSameAs(pageName)
                                     && r.Outcome.IsSameAs(VisibleOutcome));
        }

        /// <summary>
        /// Returns the zero based position of a page within the form, or -1 when it is not present.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <param name="pageName">The page name.</param>
        /// <returns>The page index or -1.</returns>
        internal static int GetPageIndex(FullFormConfig form, string pageName)
        {
            if (form?.PagesConfig == null || string.IsNullOrWhiteSpace(pageName)) return -1;
            return form.PagesConfig.FindIndex(p => p?.Name.IsSameAs(pageName) == true);
        }

        /// <summary>
        /// Validates a state name a user has typed. The name must be a simple lower case token and
        /// must not collide with any other state in the form. Because the running form state is a
        /// comma separated string that is searched with a substring match, a new name may not contain
        /// nor be contained by any other state name.
        /// </summary>
        /// <param name="form">The form the state will belong to.</param>
        /// <param name="pageName">The page that will declare the state.</param>
        /// <param name="newState">The state name entered by the user.</param>
        /// <returns>An empty string when the name is acceptable, otherwise a translation tag describing the problem.</returns>
        internal static string ValidateStateName(FullFormConfig form, string pageName, string newState)
        {
            if (string.IsNullOrWhiteSpace(newState)) return "";

            var candidate = newState.Trim();

            if (candidate.Length > MaximumStateNameLength) return "@ConStaLen@";
            if (!StateNamePattern.IsMatch(candidate)) return "@ConStaFor@";

            var otherTokens = GetOtherStateTokens(form, pageName);

            if (otherTokens.Any(t => t.IsSameAs(candidate))) return "@ConStaDup@";

            if (otherTokens.Any(t => t.IndexOf(candidate, StringComparison.OrdinalIgnoreCase) >= 0
                                     || candidate.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return "@ConStaOve@";
            }

            return "";
        }

        /// <summary>
        /// Returns every state token in the form except those declared by the supplied page.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <param name="pageName">The page to exclude.</param>
        /// <returns>The remaining state tokens.</returns>
        private static List<string> GetOtherStateTokens(FullFormConfig form, string pageName)
        {
            var declaredByPage = GetStatesForForm(form)
                .Where(s => s.DeclaredByPage.IsSameAs(pageName))
                .Select(s => s.State)
                .ToList();

            return GetAllStateTokens(form)
                .Where(t => !declaredByPage.Any(d => d.IsSameAs(t)))
                .ToList();
        }

        /// <summary>
        /// Splits a comma separated state string and adds each non empty token to the supplied list.
        /// </summary>
        /// <param name="tokens">The list to add to.</param>
        /// <param name="stateValue">The comma separated state string. May be null.</param>
        private static void AddTokens(List<string> tokens, string stateValue)
        {
            if (string.IsNullOrWhiteSpace(stateValue)) return;

            foreach (var token in stateValue.ToStringArray(","))
            {
                if (!string.IsNullOrWhiteSpace(token) && !tokens.Any(t => t.IsSameAs(token)))
                {
                    tokens.Add(token);
                }
            }
        }

        /// <summary>
        /// Returns distinct effect tokens from an on click state, falling back to the primary state
        /// when no rules are defined.
        /// </summary>
        /// <param name="onClickState">The on click state to inspect. May be null.</param>
        /// <returns>Distinct effect tokens in first-seen order.</returns>
        private static List<string> GetDistinctEffectsFromOnClickState(NextButtonClickConfig onClickState)
        {
            var effects = new List<string>();
            if (onClickState == null) return effects;

            foreach (var rule in onClickState.Rules ?? new List<RuleConfig>())
            {
                var effect = rule?.Effect?.Trim();
                if (string.IsNullOrEmpty(effect)) continue;
                if (!effects.Any(e => e.IsSameAs(effect)))
                {
                    effects.Add(effect);
                }
            }

            if (effects.Count == 0 && !string.IsNullOrWhiteSpace(onClickState.State))
            {
                effects.Add(onClickState.State.Trim());
            }

            return effects;
        }
    }
}
