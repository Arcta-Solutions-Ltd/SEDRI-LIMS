using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Command that deletes an export schedule by ID.
    /// </summary>
    internal class DeleteExportScheduleCommand : ICommand
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);
            var sql = "DELETE FROM exportschedule WHERE id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
