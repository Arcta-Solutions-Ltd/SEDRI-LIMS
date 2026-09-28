using arc.common.Models;
using arc.common.Models.Role;
using arc.data.Instruments;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Tests
{
    internal class AddCultureTestsCommand : ICommandReturningInteger
    {
        private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;
        public AddCultureTestsCommand(IInstrumentInterfaceHandler instrumentInterfaceHandler)
        {
            _instrumentInterfaceHandler = instrumentInterfaceHandler;
        }

        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var newObject = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(args[0]);
            var testList = newObject.Crafted[0].Contents;

            foreach (var test in testList)
            {
                var parameters = new { CultureId = int.Parse(args[1]), TestName = test.Key };
                if (test.Allowed == "Yes")
                {
                    var sql = @"insert into CultureTests(cultureid, testname, status, lastmodifieddate, requested)
                                        values(@CultureId, @TestName, 'Requested', now(), now())";

                    await connect.ExecuteAsync(sql, parameters);

                    await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureTestId(0, parameters.CultureId, parameters.TestName);
                }
            }

            return 0;
        }
    }
}
