using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Loads one instrument result row with status and specimen accession for the record view.
/// </summary>
internal class InstrumentResultRecordViewByIdDataQuery : IQueryReturningType<InstrumentResultRecordViewModel>
{
    async Task<InstrumentResultRecordViewModel> IQueryReturningType<InstrumentResultRecordViewModel>.ExecuteAsync(
        NpgsqlConnection connect,
        QueryFilterConfig queryFilters)
    {
        var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
        if (string.IsNullOrWhiteSpace(idParam) || !int.TryParse(idParam, out var id) || id <= 0)
            return new InstrumentResultRecordViewModel();

        // Join listitem + patient like InstrumentResultListQuery. Culture type: resolve via culture.typeid -> listitem
        // (correlated subquery avoids any join-order / alias issues with multiple listitem joins).
        const string sql = @"
SELECT
    ir.id AS Id,
    ir.instrumentprofile AS InstrumentProfile,
    COALESCE(p.firstname || ' ' || p.surname, p.surname, '') AS PatientName,
    COALESCE(li1.value, '') AS SpecimenType,
    COALESCE(
        (SELECT li_ct.value
         FROM culture c
         INNER JOIN listitem li_ct ON li_ct.id = c.typeid
         WHERE c.id = ir.cultureid
         LIMIT 1),
        ''
    ) AS CultureType,
    ir.barcode AS Barcode,
    CASE WHEN ir.requestmade IS NULL THEN '' ELSE TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS RequestMade,
    CASE WHEN ir.resultreceived IS NULL THEN '' ELSE TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS ResultReceived,
    COALESCE(li3.value, '') AS Status,
    COALESCE(ir.rawresult::text, '') AS RawResult,
    CASE WHEN ir.lastmodifieddate IS NULL THEN '' ELSE TO_CHAR(ir.lastmodifieddate AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS LastModifiedDate,
    s.accessionnumber AS AccessionNumber
FROM instrumentresults ir
INNER JOIN specimen s ON s.id = ir.specimenid
INNER JOIN patient p ON s.patientid = p.id
LEFT JOIN listitem li1 ON s.specimentypeid = li1.id
LEFT JOIN listitem li3 ON ir.statusid = li3.id
WHERE ir.id = @id";

        var row = await connect.QueryFirstOrDefaultAsync<InstrumentResultRecordViewModel>(sql, new { id });
        return row ?? new InstrumentResultRecordViewModel();
    }
}
