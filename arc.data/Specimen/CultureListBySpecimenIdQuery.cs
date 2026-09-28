using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Retrieves culture records associated with a specific specimen ID,
/// mapping each row into a <see cref="CultureListModel"/> and applying
/// any security filters defined in <see cref="QueryFilterConfig"/>.
/// </summary>
internal class CultureListBySpecimenIdQuery : IQueryReturningType<List<CultureListModel>>
{
    /// <summary>
    /// Executes the query against the provided <see cref="NpgsqlConnection"/>,
    /// extracting the specimen ID from <paramref name="queryFilters"/>,
    /// building and running the SQL that joins culture, specimen, organism,
    /// list items and alert types, then returns the fully populated list
    /// of <see cref="CultureListModel"/> instances.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> to the target PostgreSQL database.
    /// </param>
    /// <param name="queryFilters">
    /// Configuration object containing parameters (including "specimenId")
    /// and security clauses for the query.
    /// </param>
    /// <returns>
    /// A task that resolves to a list of <see cref="CultureListModel"/>
    /// representing each culture record for the specified specimen.
    /// </returns>
    public async Task<List<CultureListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetIntegerValue("specimenid");
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);
        securityClause = string.IsNullOrEmpty(securityClause) ? "" : " and sp." + securityClause;

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
			    c.Id,
			    l1.Value As specimenquantity,
			    l2.Value As orggroup,
			    cast(c.PositiveDate As varchar(10)) as PositiveDate,
			    displayonreport,
			    sp.StateId,
			    l3.Value As type,
			    c.specimenOrganismId,
			    alt.alertcategoryid,
			    alt.colour,
			    c.typeid,
			    c.specimenquantityid,
				c.growthid,
				(select parentid::text from listitemparentchild where childid = c.growthid limit 1) as GrowthTypeParentId,
				c.parentcultureid,
			    l4.Value As CommentOne,
			    l5.Value As CommentTwo,
			    c.astadditionalnotes,
			    l8.Value As ASTCommentOne,
			    l9.Value As ASTCommentTwo,
				l10.Value As Growth,
			    c.culturenumber,
				sp.specimentypeid,
				c.moredata::jsonb->>'CultureBottleWeight' as CultureBottleWeight,
			    c.moredata::jsonb->>'CultureBloodAndBottleWeight' as CultureBloodAndBottleWeight
			from
			    culture c
			    inner join specimen sp on sp.Id = c.specimenId {securityClause}
			    left outer join organism o on o.id = c.specimenorganismid
			    left outer join genus g on g.Id = o.genusId
			    left outer join species s on s.Id = o.speciesId
			    left outer join subspecies ss on ss.id = o.subspeciesId
			    left outer join serotype se on se.Id = o.serotypeId
			    left outer join additional a on a.Id = o.additionalId
			    left outer join organismsynonyms os on o.Id = os.organismId
			    and os.PreferredName = true
			    left outer join ListItem l1 on l1.id = c.specimenquantityid
			    left outer join ListItem l2 on l2.id = c.orggroupcodingid
			    left outer join ListItem l3 on l3.id = c.typeid
			    left outer join ListItem l4 on l4.id = c.commentoneid
			    left outer join ListItem l5 on l5.id = c.commenttwoid
			    left outer join ListItem l8 on l8.id = c.astcommentoneid
			    left outer join ListItem l9 on l9.id = c.astcommenttwoid
			    left outer join ListItem l10 on l10.id = c.growthid
			    left outer join AlertType alt on c.AlertTypeId = alt.id
			where
			    c.SpecimenId = @specimenId
			order by
			    c.Id
			""";

        var result = await connect.QueryAsync<CultureListModel>(sql, new { specimenId });
        var resultList = result.ToList();

        foreach (var culture in resultList)
        {
            if (!string.IsNullOrEmpty(culture.OrgGroup))
            {
                culture.SpecimenOrganism = $"{culture.OrgGroup} (group)";
            }
        }

        return resultList;
    }
}
