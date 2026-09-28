using arc.app.Common;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditBreakpointCommand : ICommandWithTypeReturningInteger<Breakpoint>
    {
        private const int CodingStatusApproved = 145;

        public async Task<int> ExecuteAsync(NpgsqlConnection connect, Breakpoint command, ILogWriter logWriter)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("id", command.Id);
            var currentBreakpoint = await new BreakpointWithOrganismQuery().ExecuteAsync(connect, queryFilter);

            var resolver = new ResolveOrganismScope<Breakpoint>();
            command = await resolver.Resolve(connect, command, currentBreakpoint);

            if (command.GenusId > 0)
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);

                command.OrganismId = await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            }

            // update breakpoint
            var sqlCheck = @"SELECT CodingStatusId FROM breakpointapproval
                            WHERE BreakpointId = @Id
                            ORDER BY Id DESC LIMIT 1";
            var latestStatus = await connect.QueryFirstOrDefaultAsync<int?>(sqlCheck, new { command.Id });
            if (latestStatus != CodingStatusApproved)
            {
                command.Enabled = "No";
            }

            var sql = @"update Breakpoint set orderid = @OrderId, familyid = @FamilyId, organismId = @OrganismId, orggroupcodingid = @OrgGroupCodingId,
                        antibioticid = @AntibioticId, testmethodid = @TestMethodId, specificationid = @SpecificationId, specialconsiderid = @SpecialConsiderId, HostId = @HostId,
                        dosage = @Dosage, enabled = @enabled, lastmodifieddate = now() Where  Id = @Id";

            await connect.ExecuteAsync(sql, command);

            // update specimen type list

            sql = "select specimentypeid from specimentypebreakpoint where breakpointId = @Id";
            var tempList = await connect.QueryAsync<int>(sql, command);

            var existingList = tempList.ToList();

            if (!string.IsNullOrEmpty(command.SpecimenTypeId))
            {
                var typeList = command.SpecimenTypeId.Split(",");
                foreach (var type in typeList)
                {
                    var typeAsNumber = int.Parse(type);
                    if (existingList.Contains(typeAsNumber))
                    {
                        existingList.Remove(typeAsNumber);
                    }
                    else
                    {
                        sql = @"insert into SpecimenTypeBreakpoint(SpecimenTypeId, breakpointId, lastmodifieddate)
                            values(@SpecimenTypeId, @BreakpointId, now())";
                        await connect.ExecuteAsync(sql, new { BreakpointId = command.Id, SpecimenTypeId = typeAsNumber });
                    }
                }
            }

            foreach (var type in existingList)
            {
                sql = @"delete from SpecimenTypeBreakpoint where BreakpointId = @BreakpointId and SpecimenTypeId = @SpecimenTypeId";
                await connect.ExecuteAsync(sql, new { BreakpointId = command.Id, SpecimenTypeId = type });
            }

            // update result lines

            sql = @"delete from ResultLine where BreakpointId = @BreakpointId";
            await connect.ExecuteAsync(sql, new { BreakpointId = command.Id });

            foreach (var breakpoint in command.BreakpointGrid)
            {
                if (breakpoint.ResultId != null && breakpoint.ResultId > 0)
                {
                    sql = @"insert into ResultLine(resultid, breakpointid, startval, endval, lastmodifieddate)
                        values (@resultId, @breakpointId, @startval, @endval, now())";
                    await connect.ExecuteAsync(sql, new { BreakpointId = command.Id, breakpoint.ResultId, breakpoint.StartVal, breakpoint.EndVal });
                }
            }

            return command.Id;
        }
    }
}
