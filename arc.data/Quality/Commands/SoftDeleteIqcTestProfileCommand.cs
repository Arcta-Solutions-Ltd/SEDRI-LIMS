using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class SoftDeleteIqcTestProfileCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, params string[] args)
        {
            var id = int.Parse(args[0]);
            var sql = @"
                update iqctestprofiles set lastmodifieddate = now(), deleteddate = now() where id = @id";

            return await connection.ExecuteAsync(sql, new { id });
        }
    }
}
