using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Returns, for each breakpoint id supplied in the comma-separated <c>ids</c> query filter, the display text
/// of the specification that breakpoint belongs to. The text uses the same composite format as
/// <see cref="SpecificationOptionsForListQuery"/> (<c>Guidelines - Document (Version, Year)</c>) so a diary
/// entry reads the same as the specification dropdown the user picked from.
/// </summary>
internal class SpecificationTextForBreakpointIdsQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain <c>ids</c> holding comma-separated breakpoint ids.</param>
    /// <returns>Options with Key = breakpoint id and Text = composite specification display string.</returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryGetStringValue("ids", out var idsValue) || string.IsNullOrWhiteSpace(idsValue))
        {
            return [];
        }

        var ids = idsValue
            .Split(',')
            .Select(part => int.TryParse(part.Trim(), out var id) ? id : 0)
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
        {
            return [];
        }

        const string sql = """
            select b.id::text as key,
                TRIM(CONCAT(
                    COALESCE(l1.value, ''),
                    ' - ',
                    COALESCE(l2.value, ''),
                    CASE WHEN l3.value IS NOT NULL OR l4.value IS NOT NULL
                        THEN CONCAT(' (', COALESCE(l3.value, ''), CASE WHEN l3.value IS NOT NULL AND l4.value IS NOT NULL THEN ', ' ELSE '' END, COALESCE(l4.value, ''), ')')
                        ELSE ''
                    END
                )) as text
            from breakpoint b
            left outer join specification s on s.id = b.specificationid
            left outer join listitem l1 on l1.id = s.guidelinesid
            left outer join listitem l2 on l2.id = s.documentid
            left outer join listitem l3 on l3.id = s.versionnumberid
            left outer join listitem l4 on l4.id = s.publicationyearid
            where b.id = ANY(@ids)
            """;

        var result = await connect.QueryAsync<OptionsConfig>(sql, new { ids });
        return result.ToList();
    }
}
