using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Data query that returns specification options for dropdown lists.
/// Each option has Key (specification id) and Text (composite display: Guidelines - Document (VersionNumber, PublicationYear)).
/// </summary>
internal class SpecificationOptionsForListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query and returns specification options for use in dropdowns.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters (unused for options list).</param>
    /// <returns>List of <see cref="OptionsConfig"/> with Key = specification id, Text = composite display string.</returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = """
            select s.id::text as key,
                TRIM(CONCAT(
                    COALESCE(l1.value, ''),
                    ' - ',
                    COALESCE(l2.value, ''),
                    CASE WHEN l3.value IS NOT NULL OR l4.value IS NOT NULL
                        THEN CONCAT(' (', COALESCE(l3.value, ''), CASE WHEN l3.value IS NOT NULL AND l4.value IS NOT NULL THEN ', ' ELSE '' END, COALESCE(l4.value, ''), ')')
                        ELSE ''
                    END
                )) as text
            from specification s
            left outer join listitem l1 on l1.id = s.guidelinesid
            left outer join listitem l2 on l2.id = s.documentid
            left outer join listitem l3 on l3.id = s.versionnumberid
            left outer join listitem l4 on l4.id = s.publicationyearid
            order by l1.value, l2.value, l3.value, l4.value
            limit 500
            """;

        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}
