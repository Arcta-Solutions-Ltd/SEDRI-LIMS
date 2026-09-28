using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class OrganismDropDownQuery : IQueryReturningType<List<OptionsConfig>>
{
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasLaboratoryId = queryFilters.TryParseIntegerValue("laboratoryid", out var laboratoryId);

        // TODO:
        // If you're logging in as a organisation without a lab, then you don't have a codingId.
        // If you don't have a codingId then you can't get a filtered list of organisms.
        // In the past this query was silently failing inside a try catch during login
        // during GetListConfiguration, line 106: _datalistFactory.GetListAsync().
        // To get login working for now we will return an empty list rather than a list of every single
        // organism in the system since it at least allows login.
        // When we hit a scenario when a user of an organisation but not a lab needs the organism list
        // we'll need to return to how this query will work.
        if (!queryFilterHasLaboratoryId)
        {
            return [];
        }

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
                lablist as (
                    select
                        unnest (string_to_array (codinglistid, ',')) as codinglistid
                    from
                        laboratory
                    where
                        id =  @laboratoryId
                )
            select
                o.Id As key,
                {organismDescriptionSql}
            from
                organism o
                inner join organismcoding oc on o.Id = oc.organismId
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
            where
                oc.codingid in (
                    select
                        cast(codinglistid as int)
                    from
                        lablist
                )
            order by
                CONCAT (g.name, s.name, ss.name, se.name, a.name)
            """;

        var result = await connect.QueryAsync<OptionsConfig>(sql, new { laboratoryId });

        return result.ToList();
    }
}
