using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class CultureViewByIdQuery : IQueryReturningType<CultureViewModel>
{
    public async Task<CultureViewModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var cultureId = queryFilters.GetIntegerValue("id");
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);

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
            end as specimenorganism
            """;

        var sql = $"""
            select
                {organismDescriptionSql},
                sp.specimentypeid,
                l1.Value As specimenquantity,
                cast(c.PositiveDate As varchar(10)) as PositiveDate,
                l2.Value as specimenapiidpanel,
                displayonreport,
                c.additionalnotes,
                c.astadditionalnotes,
                c.aloquatid,
                c.idpercentage,
                c.idprofile,
                c.positivetime,
                l4.Value As CommentOne,
                l5.Value As CommentTwo,
                l6.Value As Type,
                l7.Value As orggroup,
                l8.Value As ASTCommentOne,
                l9.Value As ASTCommentTwo,
            	l10.Value As Growth,
                c.moredata,
                (select coalesce(string_agg(cfa.fileattachmentid::text, ','), '') from culturefileattachments cfa where cfa.cultureid = c.id) as fileattachmentids
            from
                culture c
                inner join specimen sp on sp.id = c.specimenid and sp.{securityClause}
                left outer join organism o on o.id = c.specimenorganismid
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
                left outer join ListItem l1 on l1.id = c.specimenquantityid
                left outer join ListItem l2 on l2.id = c.specimenapiidpanelid
                left outer join listitem l4 on l4.id = c.commentoneid
                left outer join listitem l5 on l5.id = c.commenttwoid
                left outer join listitem l6 on l6.id = c.typeid
                left outer join ListItem l7 on l7.id = c.orggroupcodingid
                left outer join listitem l8 on l8.id = c.astcommentoneid
                left outer join listitem l9 on l9.id = c.astcommenttwoid
                left outer join ListItem l10 on l10.id = c.growthid
            where
                c.Id = @cultureId
            """;

        var result = await connect.QueryFirstAsync<CultureViewModel>(sql, new { cultureId });

        if (!string.IsNullOrEmpty(result.OrgGroup))
        {
            result.SpecimenOrganism = $"{result.OrgGroup} (group)";
        }

        return result;
    }
}
