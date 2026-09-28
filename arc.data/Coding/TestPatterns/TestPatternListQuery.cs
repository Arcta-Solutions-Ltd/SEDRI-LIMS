using arc.common.Models.Coding;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class TestPatternListQuery : IQueryReturningType<List<TestPatternListModel>>
{
    public async Task<List<TestPatternListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasTestPatternName = queryFilters.TryGetStringValue("testpatternname", out var wildcardTestPatternName);
        var queryFilterHasHostId = queryFilters.TryParseIntegerValue("hostid", out var hostId, 0);

        var whereClauseSql = new StringBuilder("where ");

        if (queryFilterHasTestPatternName)
        {
            wildcardTestPatternName = wildcardTestPatternName.Trim().ToSqlWildcard();
            var orgWhere = OrganismSearchWhereClauseUtil.GenerateWhereClause(wildcardTestPatternName, true, false);
            whereClauseSql.AppendLine($"""
                (
                    tp.testpatternname iLike '{wildcardTestPatternName}'
                    or {orgWhere}
                    or li.value ilike '{wildcardTestPatternName}'
                    or li2.value ilike '{wildcardTestPatternName}'
                )
                """);

        }

        //Filter for specimen type
        var specimenTypeJoin = "";
        if (queryFilters.TryParseIntegerValue("specimentypeid", out var specimenTypeId))
        {
            specimenTypeJoin = $"inner join specimentypetestpattern sstt on sstt.testpatternid = tp.id and sstt.specimentypeid in (@specimenTypeId) ";
        }

        if (queryFilterHasHostId && hostId > 0)
        {
            whereClauseSql.AppendLine("and li.id = @hostId");
        }

        if (whereClauseSql.Equals("where "))
        {
            whereClauseSql = new StringBuilder();
        }

        var organismDescriptionSql = """
            case
                when os.synonym is NULL then TRIM(
                    CONCAT (
                        g.name,
                        case
                            when g.name is not null
                            and s.name is null
                            and ad.name is null then ' spp.'
                            else ''
                        end,
                        ' ',
                        s.name,
                        ' ',
                        ss.name,
                        ' ',
                        TRIM(se.name),
                        ad.name
                    )
                )
                else os.synonym
            end as organismname
            """;

        var sql = $"""
            select
                {organismDescriptionSql},
            	Case When ad.familyid is NULL or ad.familyid = 0 Then ord.name Else orda.name End as ordername,
            	Case When ad.familyid is NULL or ad.familyid = 0 Then f.name Else fa.name End as familyname,
                tp.id,
                tp.testpatternname as testpatternname,
                tp.makedefault as default,
                li.value as Host,
                li2.value as orggroupname,
                STRING_AGG (li3.value, ', ') as specimentypes
            from
                testpattern tp
                left outer join organism o on o.id = tp.organismId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
                left outer join ordercat ord on ord.id = tp.orderid
                left outer join family f on f.id = tp.familyid
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional ad on ad.Id = o.additionalId
                left outer join family fa on ad.familyid = fa.id
                left outer join ordercat orda on fa.orderid = orda.id
                left outer join listitem li on tp.hostid = li.id
                left outer join listitem li2 on tp.orggroupcodingid = li2.id
                left outer join specimentypetestpattern stt on stt.testpatternid = tp.id
            	left outer join listitem li3 on stt.specimentypeid = li3.id 
                {specimenTypeJoin}
                    {whereClauseSql}
                group by testpatternname, organismname, ordername, familyname, tp.id, tp.makedefault, host, orggroupname
                order by 
                    {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "testpatternname")} 
                limit 500
            """;

        var result = await connect.QueryAsync<TestPatternListModel>(sql, new { hostId, specimenTypeId });

        return result.ToList();
    }
}
