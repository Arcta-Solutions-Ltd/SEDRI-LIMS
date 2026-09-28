using arc.app.Common;
using arc.common.Models.Coding;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditCustomEntryCommand : ICommandWithTypeReturningInteger<CustomEntryModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, CustomEntryModel command, ILogWriter logWriter)
        {
            var id = int.Parse(command.Id);

            var sql = @"select o.additionalid, oc.organismid from organism o
                        inner join organismcoding oc on oc.organismid = o.id and oc.CodingId = @CodingId
                        where o.id = @Id";
            var result = await connect.QueryAsync<OrgTypeModel>(sql, new { Id = id, CodingId = int.Parse(command.MetafCodingId) });
            var orgIds = result.FirstOrDefault();

            if (orgIds.AdditionalId != 0)
            {
                var familyId = command.GenusId == 0 ? command.FamilyId : 0;

                sql = @"update Additional a set name = @Name, familyid = @FamilyId, lastmodifieddate = now() 
                        from organism o 
                        where o.AdditionalId = a.Id and o.Id = @Id";
                await connect.ExecuteAsync(sql, new { Id = id, Name = command.Description, FamilyId = familyId });

                sql = @"update Organism set additionalId = @AdditionalId, genusid = @GenusId, speciesid = @SpeciesId, lastmodifieddate = now() where Id = @Id";
                await connect.ExecuteAsync(sql, new { Id = id, command.GenusId, command.SpeciesId });
            }

            sql = @"update OrganismCoding oc set code = @Code, lastmodifieddate = now() 
                    from organism o 
                    where o.Id = oc.organismId and o.Id = @Id and oc.CodingId = @CodingId";
            await connect.ExecuteAsync(sql, new { Id = id, command.Code, CodingId = int.Parse(command.MetafCodingId) });

            return id;
        }
    }
}
