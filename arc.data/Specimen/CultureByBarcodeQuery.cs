using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class CultureByBarcodeQuery : IQueryReturningType<CultureModel>
{
    public async Task<CultureModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var barcode = queryFilters.GetStringValue("barcode");

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
                {organismDescriptionSql},
                c.Id,
                c.SpecimenOrganismId as OrganismId,
                c.SpecimenQuantityId as Quantity,
                c.TypeId as CultureType,
                c.OrgGroupCodingId as OrgGroupCoding,
                to_char(c.PositiveDate::DATE, 'yyyy-mm-dd') As PositiveDate,
                c.PositiveTime, c.CommentOneId as Comment1,
                c.CommentTwoId as Comment2,
                c.AdditionalNotes,
                c.AloquatId as AliquotID,
                c.DisplayOnReport as DisplayInReport,
                c.moredata::jsonb->>'ManufacturersBarcode' as ManufacturersBarcode,
                x.SpecimenTypeId
            from Culture c
                left outer join organism o on o.id = c.specimenorganismid
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                left outer join organismsynonyms os on o.Id = os.organismId and os.PreferredName = true
                left outer join specimen x on x.Id = c.specimenId
            where c.moredata::jsonb->>'ManufacturersBarcode' = @barcode
            """;

        var result = await connect.QueryFirstAsync<CultureModel>(sql, new { barcode });

        result.Comment1 = result.Comment1 ?? "";
        result.Comment2 = result.Comment2 ?? "";

        return result;
    }
}
