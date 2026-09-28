using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class DeleteAlertCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = @"delete from alerttestlines where AlertId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from alertlines where AlertId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from alert where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
