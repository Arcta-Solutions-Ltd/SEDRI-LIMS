using arc.app.Common;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AddTestPatternCommand : ICommandWithTypeReturningInteger<TestPattern>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, TestPattern command, ILogWriter logWriter)
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
                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, 0, 0, command.AdditionalId);
            };

            // insert testpattern

            var sql = @"insert into TestPattern(orderid, familyid, organismid, orggroupcodingid, testpatternname, hostid, makedefault, lastmodifieddate)
                        values(@orderid, @familyid, @organismid, @orggroupcodingid, @testpatternname, @hostid, @makedefault, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var testPatternId = (int)id.id;

            // insert test pattern lines

            if (command.AntibioticGrid != null)
            {
                var order = 1;
                foreach (var testPatternLine in command.AntibioticGrid)
                {
                    testPatternLine.Dosage = testPatternLine.Dosage != null ? testPatternLine.Dosage : "";
                    sql = @"insert into TestPatternLine(testpatternid, testorder, antibioticid, dosage, testmethodid, guidelinesid, categoryid, PrintOnReport, lastmodifieddate)
                            values (@TestPatternId, @TestOrder, @AntibioticId, @Dosage, @TestMethodId, @GuidelinesId, @CategoryId, @PrintOnReport, now())";
                    await connect.ExecuteAsync(sql, new { TestPatternId = testPatternId, TestOrder = order, testPatternLine.AntibioticId,
                                                          testPatternLine.Dosage, testPatternLine.TestMethodId, testPatternLine.GuidelinesId,
                                                          testPatternLine.CategoryId, PrintOnReport = testPatternLine.PrintOnReport });
                    order++;
                }
            }

            if (!string.IsNullOrWhiteSpace(command.SpecimenTypeId))
            {
                var specimenIds = command.SpecimenTypeId.Split(",");
                logWriter.LogInfo($"Insert test pattern for specimentypes : {specimenIds}", "AddTestPatternCommand", "Execute");
                foreach (var specimen in specimenIds)
                {
                    sql = @"insert into SpecimenTypeTestPattern(SpecimenTypeId, TestPatternId, lastmodifieddate)
                            values(@SpecimenTypeId, @TestPatternId, now())";
                    await connect.ExecuteAsync(sql, new { TestPatternId = testPatternId, SpecimenTypeId = int.Parse(specimen) });
                }
            }

            return testPatternId;
        }
    }
}
