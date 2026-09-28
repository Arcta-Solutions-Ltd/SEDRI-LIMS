using arc.data.model.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Composite match for inbound instrument results using accession, machine id, profile name, and <c>moredata</c> trigger metadata.
/// </summary>
internal class InstrumentResultInboundCompositeMatchQuery : IQueryReturningType<List<InstrumentResultsDataModel>>
{
    async Task<List<InstrumentResultsDataModel>> IQueryReturningType<List<InstrumentResultsDataModel>>.ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var profileName = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("profilename", System.StringComparison.OrdinalIgnoreCase))?.Value ?? "";
        var accession = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("accessionnumber", System.StringComparison.OrdinalIgnoreCase))?.Value ?? "";
        var cultureNumber = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("culturenumber", System.StringComparison.OrdinalIgnoreCase))?.Value ?? "";
        var matchDirect = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("matchdirect", System.StringComparison.OrdinalIgnoreCase))?.Value == "true";
        var directTestName = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("directtestname", System.StringComparison.OrdinalIgnoreCase))?.Value ?? "";
        queryFilters.TryParseIntegerValue("cultureid", out var cultureId, 0);
        int? machineId = null;
        var machineParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("instrumentmachineid", System.StringComparison.OrdinalIgnoreCase))?.Value;
        if (!string.IsNullOrWhiteSpace(machineParam) && int.TryParse(machineParam.Trim(), out var mid) && mid > 0)
            machineId = mid;

        string sql;
        object param;

        if (matchDirect)
        {
            sql = """
                select * from instrumentresults ir
                where lower(trim(coalesce(ir.instrumentprofile,''))) = lower(trim(@profilename))
                  and (ir.instrumentmachineid is not distinct from @machineId)
                  and lower(trim(coalesce(ir.accessionnumber,''))) = lower(trim(@accession))
                  and coalesce(ir.moredata::jsonb->>'triggerType','') = 'directTest'
                  and lower(trim(coalesce(ir.moredata::jsonb->>'directTestName',''))) = lower(trim(@directtestname))
                  and (ir.cultureid is null or ir.cultureid = 0)
                """;
            param = new { profilename = profileName, accession, machineId, directtestname = directTestName };
        }
        else
        {
            sql = """
                select * from instrumentresults ir
                where lower(trim(coalesce(ir.instrumentprofile,''))) = lower(trim(@profilename))
                  and (ir.instrumentmachineid is not distinct from @machineId)
                  and lower(trim(coalesce(ir.accessionnumber,''))) = lower(trim(@accession))
                  and coalesce(ir.moredata::jsonb->>'triggerType','') = 'cultureType'
                  and lower(trim(coalesce(ir.culturenumber,''))) = lower(trim(@culturenumber))
                  and (@cultureid = 0 or ir.cultureid = @cultureid)
                """;
            param = new { profilename = profileName, accession, machineId, culturenumber = cultureNumber, cultureid = cultureId };
        }

        var rows = (await connect.QueryAsync<InstrumentResultsDataModel>(sql, param)).ToList();
        return rows;
    }
}
