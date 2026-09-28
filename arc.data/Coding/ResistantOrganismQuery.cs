using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class ResistantOrganismQuery : IQueryReturningType<List<OptionsConfig>>
{
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var susceptibilityId = queryFilters.GetIntegerValue("susceptibilityid");

        var organismDescriptionSql = """
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
				end as text
				""";

        var sql = $"""
			   with
			    tab as (
			        select
			            c.specimenorganismid As organismid
			        from
			            specimen s
			            inner join culture c on s.id = c.specimenid
			            inner join ast on c.id = ast.cultureid
			            and ast.susceptibilityid = @susceptibilityId
			        group by
			            c.specimenorganismid
			    )
			select
			    o.Id as key,
				{organismDescriptionSql}
			from
			    organism o
			    inner join tab t on t.organismid = o.id
			    left outer join genus g on g.Id = o.genusId
			    left outer join species s on s.Id = o.speciesId
			    left outer join subspecies ss on ss.id = o.subspeciesId
			    left outer join serotype se on se.Id = o.serotypeId
			    left outer join additional a on a.Id = o.additionalId
			    left outer join organismsynonyms os on o.Id = os.organismId
			    and os.PreferredName = true
			order by
			    text
			""";

        var result = await connect.QueryAsync<OptionsConfig>(sql, new { susceptibilityId });

        return result.ToList();
    }
}
