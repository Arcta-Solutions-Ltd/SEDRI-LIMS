using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class MarkIqcTestCompleteCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);
            var stateid = int.Parse(args[1]);

            var sql = @"update iqctests set stateid=@stateid, completeddate=now() where id=@id";
            return await connect.ExecuteAsync(sql, new { id, stateid });
        }
    }
}
