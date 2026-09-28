using arc.app.Common;
using arc.common.Models.QualityAssurance;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class AddIqcResultsToIqcTestCommand : ICommandWithTypeReturningInteger<AddIqcResultsToIqcTestModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, AddIqcResultsToIqcTestModel command, ILogWriter logWriter)
        {
            foreach (var item in command.Crafted[0].Contents)
            {
                if (item.Allowed == "Yes")
                {
                    var sql = "select id from qcantibiotics where qcorganismid = @QcOrganismId and usebydefault = true";
                    var result = await connection.QueryAsync<OrganismAntibioticModel>(sql, new { QcOrganismId = int.Parse(item.Key) });

                    foreach (var resultLine in result.ToList())
                    {
                        sql = "insert into iqcresults(iqctestid, qcantibioticid, lastmodifieddate) values(@IqcTestId, @qcantibioticid, now())";
                        await connection.ExecuteAsync(sql, new { @IqcTestId = command.IqcTestId, qcantibioticid = resultLine.Id });
                    }
                }
            }
            return 0;
        }
    }
}
