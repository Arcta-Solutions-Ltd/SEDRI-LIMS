using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class DeleteExportProfileCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            //var sql = @"delete from testpatterncoding where TestPatternId = @Id";
            //await connect.ExecuteAsync(sql, new { Id = id });

            var sql = @"delete from exportprofile where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });

        }
    }
}
