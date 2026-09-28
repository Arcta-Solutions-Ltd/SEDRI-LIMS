using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Retrieves a single <see cref="CultureModel"/> by its ID using provided query filters and security constraints.
/// </summary>
internal class CultureByIdQuery : IQueryReturningType<CultureModel>
{
    /// <summary>
    /// Executes the query to fetch a <see cref="CultureModel"/> from the database.
    /// </summary>
    /// <param name="connect">Active PostgreSQL connection.</param>
    /// <param name="queryFilters">Filter configuration containing query parameters.</param>
    /// <returns>A populated <see cref="CultureModel"/> instance or <c>null</c> if no match is found.</returns>
    public async Task<CultureModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var cultureId = queryFilters.GetIntegerValue("id");
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);
        if (! string.IsNullOrEmpty(securityClause))
        {
            securityClause = $" and x.{securityClause}";
        }

        var organismDescriptionSql = """
            Case
                When os.synonym is NULL then TRIM(
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
            end as organismname
            """;

        var sql = $"""
            select
                c.Id,
                c.ParentCultureId,
                {organismDescriptionSql},
                c.SpecimenOrganismId as OrganismId,
                c.SpecimenQuantityId as Quantity,
            	c.growthid,
                c.TypeId as CultureType,
                c.OrgGroupCodingId as OrgGroupCoding,
                to_char(c.PositiveDate::DATE, 'yyyy-mm-dd') As PositiveDate,
                c.PositiveTime,
                c.CommentOneId as Comment1,
                c.CommentTwoId as Comment2,
                c.AdditionalNotes,
                c.AloquatId as AliquotID,
                c.DisplayOnReport as DisplayInReport,
                c.idpercentage,
                x.SpecimenTypeId,
                x.LaboratoryId,
                c.moredata::jsonb->>'ManufacturersBarcode' as ManufacturersBarcode,
                c.moredata::jsonb->>'CultureBottleWeight' as CultureBottleWeight,
                c.moredata::jsonb->>'CultureBloodAndBottleWeight' as CultureBloodAndBottleWeight,
                x.Id as specimenid
            from
                Culture c
                left outer join organism o on o.id = c.specimenorganismid
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
                inner join specimen x on x.Id = c.specimenId {securityClause}
            where
                c.Id = @cultureId
            """;

        var result = await connect.QueryFirstOrDefaultAsync<CultureModel>(sql, new { cultureId });

        if (result != null)
        {
            result.Comment1 ??= "";
            result.Comment2 ??= "";
        }

        return result;
    }
}