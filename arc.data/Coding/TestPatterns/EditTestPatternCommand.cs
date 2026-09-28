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
    internal class EditTestPatternCommand : ICommandWithTypeReturningInteger<TestPattern>
    {
        /// <summary>
        /// Updates the test pattern and its antibiotic grid. Replaces all antibiotic lines for the pattern.
        /// </summary>
        /// <param name="connect">Database connection.</param>
        /// <param name="command">Test pattern with updated data including AntibioticGrid.</param>
        /// <param name="logWriter">Log writer.</param>
        /// <returns>The test pattern Id.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, TestPattern command, ILogWriter logWriter)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("id", command.Id);
            var currentTestPattern = await new EditTestPatternQuery().ExecuteAsync(connect, queryFilter);

            var resolver = new ResolveOrganismScope<TestPattern>();
            command = await resolver.Resolve(connect, command, currentTestPattern);

            if (command.GenusId > 0)
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);

                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            }

            // update test pattern
            var sql = @"update TestPattern set orderid = @orderid, familyid = @familyid, organismid = @organismid, orggroupcodingid = @orggroupcodingid,
                        testpatternname = @TestPatternName, hostid = @hostid, makedefault = @makedefault, lastmodifieddate = now() Where  Id = @Id";
            await connect.ExecuteAsync(sql, command);

            // update antibiotic list

            sql = @"delete from TestPatternLine where TestPatternId = @TestPatternId";
            await connect.ExecuteAsync(sql, new { TestPatternId = command.Id });

            if (command.AntibioticGrid != null)
            {
                var order = 1;
                foreach (var testpattern in command.AntibioticGrid)
                {
                    if (testpattern.AntibioticId != null && testpattern.AntibioticId > 0)
                    {
                        sql = @"insert into TestPatternLine(testpatternid, testorder, antibioticid, dosage, testmethodid, guidelinesid, categoryid, printonreport, lastmodifieddate)
                        values (@TestPatternId, @TestOrder, @AntibioticId, @Dosage, @TestMethodId, @GuidelinesId, @CategoryId, @PrintOnReport, now())";
                        await connect.ExecuteAsync(sql, new
                        {
                            TestPatternId = command.Id,
                            TestOrder = order,
                            testpattern.AntibioticId,
                            testpattern.Dosage,
                            testpattern.TestMethodId,
                            testpattern.GuidelinesId,
                            testpattern.CategoryId,
                            PrintOnReport = testpattern.PrintOnReport
                        });
                        order++;
                    }
                }
            }

            // update specimen type list

            sql = "select specimentypeid from specimentypetestpattern where TestPatternId = @Id";
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
                        sql = @"insert into SpecimenTypeTestPattern(SpecimenTypeId, testpatternId, lastmodifieddate)
                            values(@SpecimenTypeId, @TestPatternId, now())";
                        await connect.ExecuteAsync(sql, new { TestPatternId = command.Id, SpecimenTypeId = typeAsNumber });
                    }
                }
            }

            foreach (var type in existingList)
            {
                sql = @"delete from SpecimenTypeTestPattern where TestPatternId = @TestPatternId and SpecimenTypeId = @SpecimenTypeId";
                await connect.ExecuteAsync(sql, new { TestPatternId = command.Id, SpecimenTypeId = type });
            }

            return command.Id;
        }
    }
}
