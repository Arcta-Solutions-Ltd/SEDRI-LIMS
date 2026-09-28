using arc.app.Common;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AddBreakpointCommand : ICommandWithTypeReturningInteger<Breakpoint>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, Breakpoint command, ILogWriter logWriter)
        {

            if (command.OrganismId > 0)
            {
                var hierachyUtil = new GetHierarchyFromOrganismId();
                var hierarchy = await hierachyUtil.ExecuteAsync(connect, new QueryFilterConfig().AddInteger("Id", command.OrganismId));
                command.OrderId = hierarchy.OrderId;
                command.FamilyId = hierarchy.FamilyId;
                command.GenusId = hierarchy.GenusId;
                command.SpeciesId = hierarchy.SpeciesId;
                command.AdditionalId = hierarchy.AdditionalId;
            }
            else
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);
                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            };

            // insert breakpoint

            logWriter.LogInfo("Insert breakpoint", "AddBreakpointCommand", "Execute");
            var sql = @"insert into Breakpoint(orderid, familyid, organismid, orggroupcodingid, antibioticid, testmethodid, specificationid, specialconsiderid, hostid, dosage, enabled, lastmodifieddate)
                        values(@orderid, @familyid, @organismid, @orggroupcodingid, @antibioticid, @testmethodid, @specificationid, @specialconsiderid, @hostid, @dosage, @enabled, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var breakpointId = (int)id.id;

            // insert specimen types

            if ( ! string.IsNullOrWhiteSpace(command.SpecimenTypeId))
            {
                var specimenIds = command.SpecimenTypeId.Split(",");
                logWriter.LogInfo($"Insert breakpoints for specimentypes : {specimenIds}", "AddBreakpointCommand", "Execute");
                foreach (var specimen in specimenIds)
                {
                    sql = @"insert into SpecimenTypeBreakpoint(SpecimenTypeId, breakpointId, lastmodifieddate)
                            values(@SpecimenTypeId, @BreakpointId, now())";
                    await connect.ExecuteAsync(sql, new { BreakpointId = breakpointId, SpecimenTypeId = int.Parse(specimen) });
                }
            }

            // insert results

            if (command.BreakpointGrid != null)
            {
                logWriter.LogInfo("Insert breakpoint lines", "AddBreakpointCommand", "Execute");
                foreach (var breakpoint in command.BreakpointGrid)
                {
                    sql = @"insert into ResultLine(resultid, breakpointid, startval, endval, lastmodifieddate)
                        values (@resultId, @breakpointId, @startval, @endval, now())";
                    await connect.ExecuteAsync(sql, new { BreakpointId = breakpointId, breakpoint.ResultId, breakpoint.StartVal, breakpoint.EndVal });
                }
            }

            return breakpointId;
        }
    }
}
