using arc.app.Common;
using arc.common.Models.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class SynonymCommand : ICommandWithTypeReturningInteger<EditSynonymModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, EditSynonymModel command, ILogWriter logWriter)
        {
            var sql = @"delete from organismsynonyms where organismId = @OrganismId";
            await connect.ExecuteAsync(sql, new { OrganismId = command.Id });

            if (! string.IsNullOrEmpty(command.PreferredName))
            {
                sql = "insert into organismsynonyms(OrganismId, Synonym, PreferredName, LastModifiedDate) Values(@OrganismId, @Synonym, @PreferredName, now())";
                await connect.ExecuteAsync(sql, new { OrganismId = command.Id, Synonym = command.PreferredName, PreferredName = true });
            }

            if (command.SynonymGrid != null)
            {
                foreach (var synonym in command.SynonymGrid)
                {
                    sql = "insert into organismsynonyms(OrganismId, Synonym, PreferredName, LastModifiedDate) Values(@OrganismId, @Synonym, @PreferredName, now())";
                    await connect.ExecuteAsync(sql, new { OrganismId = command.Id, Synonym = synonym.Synonym, PreferredName = false });
                }
            }

            return 0;
        }
    }
}
