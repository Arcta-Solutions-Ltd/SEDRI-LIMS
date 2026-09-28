using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a query to retrieve a list of instrument requests based on profiles and batch information.
/// Implements the <see cref="IQueryReturningType{T}"/> interface.
/// </summary>
internal class InstrumentRequestQuery : IQueryReturningType<List<InstrumentRequestModel>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a list of instrument request models
    /// filtered by profiles and batch parameters. Optional filter <c>InstrumentMachineId</c> (case-insensitive)
    /// restricts rows to that machine list id when present and greater than zero.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="queryFilters">The query filter configuration containing parameters for profiles and batch.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of
    /// <see cref="InstrumentRequestModel"/> objects with the retrieved data.
    /// </returns>
    public async Task<List<InstrumentRequestModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var profiles = queryFilters.Parameters.Where(p => p.Key.ToLower() == "profiles").First();
        var batch = queryFilters.Parameters.Where(p => p.Key.ToLower() == "batch").First();
        var machineParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "instrumentmachineid");
        int? instrumentMachineId = null;
        if (machineParam != null && int.TryParse(machineParam.Value, out var mid) && mid > 0)
            instrumentMachineId = mid;

        const string blaListItemIdExpr = @"
            CASE
                WHEN ct.testresults IS NULL THEN NULL
                WHEN COALESCE(
                    NULLIF(TRIM(ct.testresults::jsonb->>'betalactamaseresultId'), ''),
                    NULLIF(TRIM(ct.testresults::jsonb->>'betalactamaseresultid'), ''),
                    NULLIF(TRIM(ct.testresults::jsonb->>'BetalactamaseResultId'), '')
                ) ~ '^[0-9]+$' THEN (
                    COALESCE(
                        NULLIF(TRIM(ct.testresults::jsonb->>'betalactamaseresultId'), ''),
                        NULLIF(TRIM(ct.testresults::jsonb->>'betalactamaseresultid'), ''),
                        NULLIF(TRIM(ct.testresults::jsonb->>'BetalactamaseResultId'), '')
                    )
                )::integer
                ELSE NULL
            END";

        var baseSql = $@"select ir.id, ir.specimenid, ir.cultureid, s.accessionnumber, p.patientref, ir.barcode, ir.instrumentprofile as instrumentname,
                        coalesce(c.culturenumber::text, '') as culturenumber, oc.code as organismcode, p.firstname, p.surname as lastname,
                        ir.instrumentmachineid as instrumentmachineid,
                        {VitekCollectedDateTimeSql.SelectExpression},
                        coalesce(st.value, '') as specimentypename,
                        ct.completed as blacompleted,
                        ct.status as blastatus,
                        bl.value as blalistitemvalue
                        from instrumentresults ir
                        inner join specimen s on s.id = ir.specimenid
                        inner join patient p on p.id = s.patientid
                        left join listitem st on st.id = s.specimentypeid
                        left join culture c on c.id = ir.cultureid
                        left outer join organism o on o.id = c.specimenorganismid
                        left outer join organismcoding oc on o.id = oc.organismid and oc.codingid = 10007
                        left join culturetests ct on ct.cultureid = ir.cultureid and lower(trim(ct.testname)) = 'betalactamasetestform'
                        left join listitem bl on bl.id = ({blaListItemIdExpr})
                        where ir.statusid = 882 and LOWER(ir.instrumentprofile) in (@Profiles)";

        var batchSize = int.TryParse(batch.Value, out var bs) ? bs : 10;

        IEnumerable<InstrumentRequestQueryRow> rows;
        if (instrumentMachineId.HasValue)
        {
            var sql = baseSql + " and ir.instrumentmachineid = @InstrumentMachineId limit @BatchSize";
            rows = await connect.QueryAsync<InstrumentRequestQueryRow>(sql, new { Profiles = profiles.Value, BatchSize = batchSize, InstrumentMachineId = instrumentMachineId.Value });
        }
        else
        {
            var sql = baseSql + " limit @BatchSize";
            rows = await connect.QueryAsync<InstrumentRequestQueryRow>(sql, new { Profiles = profiles.Value, BatchSize = batchSize });
        }

        return rows.Select(ToModel).ToList();
    }

    private static InstrumentRequestModel ToModel(InstrumentRequestQueryRow row)
    {
        var specimenTypeName = row.SpecimenTypeName ?? "";
        return new InstrumentRequestModel
        {
            Id = row.Id,
            SpecimenId = row.SpecimenId,
            CultureId = row.CultureId,
            AccessionNumber = row.AccessionNumber ?? "",
            PatientRef = row.PatientRef ?? "",
            Barcode = row.Barcode ?? "",
            CultureNumber = row.CultureNumber ?? "",
            OrganismCode = row.OrganismCode ?? "",
            FirstName = row.FirstName ?? "",
            LastName = row.LastName ?? "",
            InstrumentName = row.InstrumentName ?? "",
            InstrumentMachineId = row.InstrumentMachineId,
            CollectedDateTime = string.IsNullOrEmpty(row.CollectedDateTime) ? "00000000000000" : row.CollectedDateTime,
            SpecimenTypeName = specimenTypeName,
            BlaTestJson = VitekOutboundInstrumentRequestBuilder.BuildBlaTestJson(row.BlaCompleted, row.BlaStatus ?? "", row.BlaListItemValue ?? "")
        };
    }
}
