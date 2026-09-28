using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads a single organism by its primary key and returns it as an <see cref="OrganismListModel"/>,
/// including a computed description (genus/species/subspecies/serotype/additional or preferred synonym) and custom flag.
/// </summary>
internal class OrganismByIdQuery : IQueryReturningType<OrganismListModel>
{
    /// <summary>
    /// Executes the query to fetch the organism with the given id from the query filters.
    /// </summary>
    /// <param name="connect">Active PostgreSQL connection.</param>
    /// <param name="queryFilters">Filters containing the organism "id" to load.</param>
    /// <returns>The organism row as <see cref="OrganismListModel"/>, or throws if not found.</returns>
    public async Task<OrganismListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var organismId = queryFilters.GetIntegerValue("id");

        // Build description: preferred synonym if set, otherwise concatenated genus/species/subspecies/serotype/additional (with "spp." when only genus).
        var organismDescriptionSql = $"""
            case
                when os.synonym is NULL then TRIM(
                    CONCAT (
                        g.name,
                        case
                            when g.name is not null
                            and s.name is null
                            and a.name is null then ' spp.'
                            else ''
                        end,
                        ' ',
                        s.name,
                        ' ',
                        ss.name,
                        ' ',
                        TRIM(se.name),
                        a.name
                    )
                )
                else os.synonym
            end as description
            """;

        // Select description, Id, and custom (Yes/No from additional name) for the organism by id.
        var sql = $"""
            select
                {organismDescriptionSql},
                o.Id,
                Case
                    When a.name is NULL Then 'No'
                    Else 'Yes'
                End As custom
            From
                Organism o
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
            where
                o.Id = @organismId
            """;

        return await connect.QueryFirstAsync<OrganismListModel>(sql, new { organismId });
    }
}
