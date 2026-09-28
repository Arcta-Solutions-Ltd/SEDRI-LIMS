using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Configuration
{
    /// <summary>
    /// A group as it actually appears on one form: the catalogue definition plus the pages from
    /// this form that belong to it.
    /// </summary>
    internal sealed class PageGroupInstance
    {
        /// <summary>Gets the catalogue definition for this group.</summary>
        internal PageGroupDefinition Definition { get; init; }

        /// <summary>Gets the anchor page names in declared anchor order.</summary>
        internal List<string> AnchorPages { get; init; }

        /// <summary>Gets the freely orderable member page names.</summary>
        internal List<string> MemberPages { get; init; }

        /// <summary>Gets every page name in the group, anchors first.</summary>
        internal IEnumerable<string> AllPages => AnchorPages.Concat(MemberPages);
    }

    /// <summary>Which page group invariant was broken.</summary>
    internal enum PageGroupViolation
    {
        /// <summary>A foreign page sits inside a group, or the group's own pages are not consecutive.</summary>
        GroupSplit,

        /// <summary>An anchor page is not in its declared position at the start of the group.</summary>
        AnchorMoved,

        /// <summary>A member page appears before an anchor of the same group.</summary>
        MemberBeforeAnchor,

        /// <summary>Two groups appear in the wrong canonical sequence.</summary>
        GroupSequence
    }

    /// <summary>
    /// One broken invariant, carrying the page name ids involved for logging and messaging.
    /// </summary>
    internal sealed class PageGroupBreach
    {
        /// <summary>Gets which invariant failed.</summary>
        internal PageGroupViolation Reason { get; init; }

        /// <summary>Gets the group id, when the breach is specific to one group.</summary>
        internal string GroupId { get; init; }

        /// <summary>Gets the page name ids involved in the breach.</summary>
        internal List<string> PageNames { get; init; }

        /// <summary>Gets the translation tag to show the user for this breach.</summary>
        internal string MessageTag => PageGroupUtils.MessageTagFor(Reason);
    }

    /// <summary>
    /// Builds, validates and repairs form page order against the page group invariants.
    /// A group exists on a form only when that form contains at least one of the group's anchor
    /// pages; member pages on a form without the matching anchor are treated as ungrouped.
    /// </summary>
    internal static class PageGroupUtils
    {
        /// <summary>
        /// Returns the translation tag for a page group violation.
        /// </summary>
        /// <param name="reason">The broken invariant.</param>
        /// <returns>A language tag to show the user.</returns>
        internal static string MessageTagFor(PageGroupViolation reason) => reason switch
        {
            PageGroupViolation.AnchorMoved => "@ConPagM@",
            PageGroupViolation.MemberBeforeAnchor => "@ConPagM@",
            PageGroupViolation.GroupSequence => "@ConPagN@",
            _ => "@ConPagL@"
        };

        /// <summary>
        /// Resolves a form name from a configuration record id. Ids are either the form name or
        /// <c>view|formName</c>; matching always uses the form name id, never a translated title.
        /// </summary>
        /// <param name="idSource">The record identifier from the query or the save payload.</param>
        /// <returns>The form name, or null when the source is empty.</returns>
        internal static string ResolveFormName(string idSource)
        {
            if (string.IsNullOrWhiteSpace(idSource))
            {
                return null;
            }

            var parts = idSource.Split('|');
            if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                return parts[1].Trim();
            }

            return parts[0].Trim();
        }

        /// <summary>
        /// Returns true when the form contains at least one anchor page for the given group.
        /// </summary>
        /// <param name="form">The form to inspect.</param>
        /// <param name="groupId">The group id to look for.</param>
        /// <returns>True when the group is present on this form.</returns>
        internal static bool FormHasGroup(FullFormConfig form, string groupId)
        {
            if (form?.PagesConfig == null || string.IsNullOrWhiteSpace(groupId))
            {
                return false;
            }

            return form.PagesConfig.Any(p =>
                p != null &&
                p.GroupAnchor > 0 &&
                string.Equals(p.PageGroup, groupId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Sets <see cref="PageConfig.PageGroup"/> from the page table name when the form already
        /// contains that group. Anchor pages keep the group they were given by the factory.
        /// </summary>
        /// <param name="page">The page to update.</param>
        /// <param name="form">The form that owns the page.</param>
        internal static void AssignGroupFromTableName(PageConfig page, FullFormConfig form)
        {
            if (page == null || form == null || page.GroupAnchor > 0)
            {
                return;
            }

            var definition = PageGroupCatalogue.ByTableName(page.TableName);
            if (definition != null && FormHasGroup(form, definition.Id))
            {
                page.PageGroup = definition.Id;
                return;
            }

            page.PageGroup = null;
        }

        /// <summary>
        /// Builds the groups that actually appear on the form, from the page definitions.
        /// Groups without an anchor on this form are omitted so their member pages stay ungrouped.
        /// </summary>
        /// <param name="form">The form whose pages supply group membership.</param>
        /// <returns>Group instances in canonical sequence.</returns>
        internal static List<PageGroupInstance> BuildGroups(FullFormConfig form)
        {
            var result = new List<PageGroupInstance>();
            if (form?.PagesConfig == null)
            {
                return result;
            }

            foreach (var definition in PageGroupCatalogue.All)
            {
                var grouped = form.PagesConfig
                    .Where(p => p != null
                        && !string.IsNullOrWhiteSpace(p.Name)
                        && string.Equals(p.PageGroup, definition.Id, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var anchors = grouped
                    .Where(p => p.GroupAnchor > 0)
                    .OrderBy(p => p.GroupAnchor)
                    .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                    .Select(p => p.Name)
                    .ToList();

                if (anchors.Count == 0)
                {
                    continue;
                }

                var members = grouped
                    .Where(p => p.GroupAnchor <= 0)
                    .Select(p => p.Name)
                    .ToList();

                result.Add(new PageGroupInstance
                {
                    Definition = definition,
                    AnchorPages = anchors,
                    MemberPages = members
                });
            }

            return result;
        }

        /// <summary>
        /// Checks a proposed page name order against the group invariants.
        /// </summary>
        /// <param name="form">The form whose page definitions supply group membership.</param>
        /// <param name="proposedOrder">The submitted page name order.</param>
        /// <returns>Every broken invariant; empty when the order is acceptable.</returns>
        internal static List<PageGroupBreach> Validate(FullFormConfig form, IReadOnlyList<string> proposedOrder)
        {
            var breaches = new List<PageGroupBreach>();
            if (form == null || proposedOrder == null)
            {
                return breaches;
            }

            var groups = BuildGroups(form);
            if (groups.Count == 0)
            {
                return breaches;
            }

            var appearances = new List<(PageGroupInstance Group, int FirstIndex, int LastIndex)>();

            foreach (var group in groups)
            {
                var groupNames = group.AllPages
                    .Where(name => IndexOfPage(proposedOrder, name) >= 0)
                    .ToList();
                if (groupNames.Count == 0)
                {
                    continue;
                }

                var indices = groupNames.Select(name => IndexOfPage(proposedOrder, name)).OrderBy(i => i).ToList();
                var first = indices[0];
                var last = indices[^1];
                if (last - first + 1 != indices.Count)
                {
                    breaches.Add(new PageGroupBreach
                    {
                        Reason = PageGroupViolation.GroupSplit,
                        GroupId = group.Definition.Id,
                        PageNames = groupNames
                    });
                    continue;
                }

                var slice = proposedOrder.Skip(first).Take(last - first + 1).ToList();
                appearances.Add((group, first, last));

                var expectedAnchors = group.AnchorPages
                    .Where(name => IndexOfPage(proposedOrder, name) >= 0)
                    .ToList();

                for (var i = 0; i < expectedAnchors.Count; i++)
                {
                    if (i >= slice.Count || !PageNamesEqual(slice[i], expectedAnchors[i]))
                    {
                        var reason = slice.Take(expectedAnchors.Count).Any(name =>
                            group.MemberPages.Any(member => PageNamesEqual(member, name)))
                            ? PageGroupViolation.MemberBeforeAnchor
                            : PageGroupViolation.AnchorMoved;

                        breaches.Add(new PageGroupBreach
                        {
                            Reason = reason,
                            GroupId = group.Definition.Id,
                            PageNames = slice
                        });
                        break;
                    }
                }
            }

            var orderedAppearances = appearances.OrderBy(a => a.FirstIndex).ToList();
            for (var i = 1; i < orderedAppearances.Count; i++)
            {
                var previous = orderedAppearances[i - 1].Group.Definition.Sequence;
                var current = orderedAppearances[i].Group.Definition.Sequence;
                if (current < previous)
                {
                    breaches.Add(new PageGroupBreach
                    {
                        Reason = PageGroupViolation.GroupSequence,
                        GroupId = orderedAppearances[i].Group.Definition.Id,
                        PageNames = orderedAppearances.Select(a => a.Group.Definition.Id).ToList()
                    });
                    break;
                }
            }

            return breaches;
        }

        /// <summary>
        /// Returns a page order that satisfies the group invariants, keeping member order and
        /// ungrouped page order from <paramref name="proposedOrder"/> where possible.
        /// </summary>
        /// <param name="form">The form whose page definitions supply group membership.</param>
        /// <param name="proposedOrder">The current or requested page name order.</param>
        /// <returns>A valid page name order covering every page on the form.</returns>
        internal static List<string> Normalise(FullFormConfig form, IReadOnlyList<string> proposedOrder)
        {
            var existing = form?.Pages?.Where(name => !string.IsNullOrWhiteSpace(name)).ToList()
                ?? new List<string>();
            var order = (proposedOrder ?? existing)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            foreach (var name in existing)
            {
                if (IndexOfPage(order, name) < 0)
                {
                    order.Add(name);
                }
            }

            var groups = BuildGroups(form);
            if (groups.Count == 0)
            {
                return order;
            }

            var result = new List<string>();
            var emitted = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void EmitGroup(PageGroupInstance group)
            {
                if (!emitted.Add(group.Definition.Id))
                {
                    return;
                }

                foreach (var anchor in group.AnchorPages)
                {
                    if (IndexOfPage(order, anchor) >= 0)
                    {
                        result.Add(anchor);
                    }
                }

                foreach (var pageName in order)
                {
                    if (group.MemberPages.Any(member => PageNamesEqual(member, pageName)))
                    {
                        result.Add(pageName);
                    }
                }
            }

            foreach (var pageName in order)
            {
                if (IndexOfPage(result, pageName) >= 0)
                {
                    continue;
                }

                var owningGroup = groups.FirstOrDefault(g =>
                    g.AllPages.Any(name => PageNamesEqual(name, pageName)));
                if (owningGroup == null)
                {
                    result.Add(pageName);
                    continue;
                }

                foreach (var earlier in groups)
                {
                    if (earlier.Definition.Sequence < owningGroup.Definition.Sequence)
                    {
                        EmitGroup(earlier);
                    }
                }

                EmitGroup(owningGroup);
            }

            foreach (var group in groups)
            {
                EmitGroup(group);
            }

            return result;
        }

        /// <summary>
        /// Writes a page name order onto the form page list and reorders the page configurations
        /// to match.
        /// </summary>
        /// <param name="form">The form to update.</param>
        /// <param name="order">The page name order to apply.</param>
        internal static void ApplyOrder(FullFormConfig form, IReadOnlyList<string> order)
        {
            if (form == null || order == null)
            {
                return;
            }

            form.Pages = order.ToList();
            if (form.PagesConfig == null)
            {
                return;
            }

            var newPagesConfig = new List<PageConfig>();
            foreach (var pageName in order)
            {
                var page = form.GetPage(pageName);
                if (page != null)
                {
                    newPagesConfig.Add(page);
                }
            }

            foreach (var page in form.PagesConfig)
            {
                if (page != null && !newPagesConfig.Contains(page))
                {
                    newPagesConfig.Add(page);
                }
            }

            form.PagesConfig = newPagesConfig;
        }

        /// <summary>
        /// Describes the group layout for a log line, using page name ids.
        /// </summary>
        /// <param name="form">The form whose groups to describe.</param>
        /// <returns>A compact layout string for diagnostics.</returns>
        internal static string DescribeLayout(FullFormConfig form)
        {
            var groups = BuildGroups(form);
            if (groups.Count == 0)
            {
                return "none";
            }

            return string.Join("; ", groups.Select(g =>
                $"{g.Definition.Id}:anchors=[{string.Join(", ", g.AnchorPages)}] members=[{string.Join(", ", g.MemberPages)}]"));
        }

        private static int IndexOfPage(IReadOnlyList<string> order, string pageName)
        {
            for (var i = 0; i < order.Count; i++)
            {
                if (PageNamesEqual(order[i], pageName))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool PageNamesEqual(string left, string right) =>
            string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}
