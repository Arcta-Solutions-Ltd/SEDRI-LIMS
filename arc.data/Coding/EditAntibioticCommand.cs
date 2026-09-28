using arc.app.Common;
using arc.domain.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditAntibioticCommand : ICommandWithTypeReturningInteger<Antibiotic>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, Antibiotic command, ILogWriter logWriter)
        {
            var sql = @"update antibiotic ant set AntibioticName = @AntibioticName, Code = @Code, Atc = @Atc, Cid = @Cid, Loinc = @Loinc, LastModifiedDate = now()
                        from AntibioticCoding ac 
                        where ac.antibioticid = ant.id and ac.Id = @Id ";

            await connect.ExecuteAsync(sql, command);

            return command.Id;
        }
    }
}
