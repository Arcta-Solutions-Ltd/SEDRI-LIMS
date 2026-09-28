using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    /// <summary>
    /// Command that deletes a coding list entry from <c>listitem</c>
    /// and its relations in <c>organismcoding</c>.
    /// </summary>
    internal class DeleteCodingListCommand : ICommand
    {
        /// <summary>
        /// Executes the delete operations for the provided coding <c>Id</c>.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="args">
        /// Command arguments where <c>args[0]</c> is the coding identifier.
        /// </param>
        /// <returns>The number of rows affected by the final delete.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var sql = @"delete from listitem where id = @Id";
            await connect.ExecuteAsync(sql, new { Id = int.Parse(args[0]) });

            sql = @"delete from organismcoding where codingid = @Id";
            return await connect.ExecuteAsync(sql, new { Id = int.Parse(args[0]) });
        }
    }
}
