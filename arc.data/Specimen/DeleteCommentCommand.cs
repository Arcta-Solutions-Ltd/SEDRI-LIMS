using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    internal class DeleteCommentCommand : ICommand
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = @"delete from specimencomment where id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });        
        }
    }
}
