using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig
{
    /// <summary>
    /// Defines a page group: the pages that must stay together in a form's page order, the entity
    /// table that recruits user added pages into the group, and the group's place in the canonical
    /// group sequence.
    /// </summary>
    public sealed class PageGroupDefinition
    {
        /// <summary>
        /// Gets the group id stored on <see cref="PageConfig.PageGroup"/>, for example <c>patient</c>.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the translation tag used as the group heading in the Page Order editor.
        /// </summary>
        public string TitleTag { get; init; }

        /// <summary>
        /// Gets the page table name that puts a user added page into this group, or null when the
        /// group accepts no user added pages.
        /// </summary>
        public string TableName { get; init; }

        /// <summary>
        /// Gets the group's rank in the canonical group sequence. Groups must appear in a form's
        /// page order in ascending sequence.
        /// </summary>
        public int Sequence { get; init; }
    }

    /// <summary>
    /// Read only catalogue of the page groups the system understands. Groups exist only where a
    /// built in anchor page declares them, so a form without an anchor page has no group.
    /// </summary>
    public static class PageGroupCatalogue
    {
        /// <summary>Group id for the patient search, results and patient detail pages.</summary>
        public const string PatientGroup = "patient";

        /// <summary>Group id for the admission selection and admission detail pages.</summary>
        public const string AdmissionGroup = "admission";

        /// <summary>Group id for the request selection and request detail pages.</summary>
        public const string RequestGroup = "request";

        /// <summary>Group id for the organism search and organism selection pages.</summary>
        public const string OrganismGroup = "organism";

        private static readonly List<PageGroupDefinition> Definitions = new()
        {
            new PageGroupDefinition { Id = PatientGroup, TitleTag = "@PatPatI@", TableName = "patient", Sequence = 10 },
            new PageGroupDefinition { Id = AdmissionGroup, TitleTag = "@NeoAdm@", TableName = "admission", Sequence = 20 },
            new PageGroupDefinition { Id = RequestGroup, TitleTag = "@NeoReq@", TableName = "request", Sequence = 30 },
            new PageGroupDefinition { Id = OrganismGroup, TitleTag = "@GenOrgA@", TableName = null, Sequence = 40 }
        };

        /// <summary>
        /// Gets every known group definition in ascending canonical sequence.
        /// </summary>
        public static IReadOnlyList<PageGroupDefinition> All => Definitions;

        /// <summary>
        /// Finds a group definition by its group id.
        /// </summary>
        /// <param name="groupId">The group id held on a page configuration.</param>
        /// <returns>The matching definition, or null when the id is empty or unknown.</returns>
        public static PageGroupDefinition ById(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return null;
            }

            return Definitions.FirstOrDefault(d => string.Equals(d.Id, groupId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds the group a user added page joins when it targets the supplied table.
        /// </summary>
        /// <param name="tableName">The page table name chosen on the add or edit page form.</param>
        /// <returns>The matching definition, or null when the table does not belong to a group.</returns>
        public static PageGroupDefinition ByTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            return Definitions.FirstOrDefault(d =>
                d.TableName != null && string.Equals(d.TableName, tableName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the canonical sequence for a group id, used to keep group blocks in workflow order.
        /// </summary>
        /// <param name="groupId">The group id held on a page configuration.</param>
        /// <returns>The group sequence, or <see cref="int.MaxValue"/> when the group is unknown.</returns>
        public static int SequenceOf(string groupId)
        {
            return ById(groupId)?.Sequence ?? int.MaxValue;
        }
    }
}
