using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class DeleteConfigCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var sql = @"delete from Configs Where  ConfigName = @ConfigName";

            return await connect.ExecuteAsync(sql, new { ConfigName = args[0] });
        }
    }
}
