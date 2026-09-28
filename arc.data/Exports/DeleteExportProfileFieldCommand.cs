using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class DeleteExportProfileFieldCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);
            var sql = @"delete from exportprofilerecord where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
