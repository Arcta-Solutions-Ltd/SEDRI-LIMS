using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class DeleteTestPatternCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = @"delete from testPatternLine where TestPatternId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from specimentypetestpattern where TestPatternId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from testpattern where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
