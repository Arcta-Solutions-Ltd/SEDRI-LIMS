using arc.app.Common;
using arc.domain.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AddAntibioticCommand : ICommandWithTypeReturningInteger<Antibiotic>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, Antibiotic command, ILogWriter logWriter)
        {

            // insert breakpoint

            logWriter.LogInfo("Insert antibiotic", "AddAntibioticCommand", "Execute");
            var sql = @"insert into Antibiotic(code, antibioticname, atc, cid, loinc, lastmodifieddate)
                        values(@code, @antibioticname, @atc, @cid, @loinc, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var antibioticId = (int)id.id;

            sql = @"insert into AntibioticCoding(code, antibioticid, codingid, lastmodifieddate)
                        values(@code, @antibioticid, @codingid, now())";
            await connect.ExecuteAsync(sql, new { Code = command.Code, AntibioticId = antibioticId, CodingId = 21});

            return antibioticId;
        }
    }
}
