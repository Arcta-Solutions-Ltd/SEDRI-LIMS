using arc.app.Common;
using arc.common.Models.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AddCustomEntryCommand : ICommandWithTypeReturningInteger<CustomEntryModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, CustomEntryModel command, ILogWriter logWriter)
        {
            var familyId = command.GenusId == 0 ? command.FamilyId : 0;

            var sql = @"insert into Additional(name, familyid, lastmodifieddate) values(@name, @FamilyId, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, new { Name = command.Description, FamilyId = familyId });
            var additionalId = (int)id.id;

            sql = @"insert into Organism(additionalId, genusid, speciesid, lastmodifieddate) values(@AdditionalId, @GenusId, @SpeciesId, now()) returning id";
            id = await connect.QueryFirstAsync(sql, new { AdditionalId = additionalId, GenusId = command.GenusId, SpeciesId = command.SpeciesId });
            var organismId = (int)id.id;

            sql = @"insert into OrganismCoding(organismId, codingid, code, lastmodifieddate) values(@OrganismId, @CodingId, @Code, now()) returning id";
            await connect.QueryFirstAsync(sql, new { OrganismId = organismId, CodingId = int.Parse(command.MetafCodingId), Code = command.Code });

            return organismId;
        }
    }
}
