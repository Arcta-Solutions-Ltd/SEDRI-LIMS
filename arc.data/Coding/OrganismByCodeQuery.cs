using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class OrganismByCodeQuery : IQueryReturningType<OrganismListModel>
{
    public async Task<OrganismListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var codingId = queryFilters.GetIntegerValue("codingid");
        var code = queryFilters.GetStringValue("code");

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
            end as description
            """;

        var sql = $"""
            select
                {organismDescriptionSql},
                o.Id,
            case
                    When a.name is NULL Then 'No'
                    Else 'Yes'
                End As custom
            from
                Organism o
                inner join OrganismCoding oc on oc.organismid = o.id
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
            where
                oc.CodingId = @codingId
                and oc.code = @code
            """;

        return await connect.QueryFirstOrDefaultAsync<OrganismListModel>(sql, new { codingId, code });
    }
}
