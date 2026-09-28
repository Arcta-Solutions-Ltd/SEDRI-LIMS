using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class DeleteExpertRuleCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = @"delete from expertruletestcondition where ExpertRuleId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from expertrulecondition where ExpertRuleId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from expertruleaction where ExpertRuleId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from expertrulespecimentype where ExpertRuleId = @Id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from expertrule where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });


        }
    }
}
