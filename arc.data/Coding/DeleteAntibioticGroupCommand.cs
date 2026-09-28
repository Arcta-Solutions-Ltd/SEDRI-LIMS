using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    /// <summary>
    /// Command that deletes an antibiotic group entry from the coding list
    /// and removes associated rows from the <c>antibioticcoding</c> table.
    /// </summary>
    internal class DeleteAntibioticGroupCommand : ICommand
    {
        /// <summary>
        /// Executes the delete operations for the specified coding <c>Id</c>.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="args">
        /// Command arguments where <c>args[0]</c> is the identifier of the
        /// coding entry to delete.
        /// </param>
        /// <returns>
        /// The number of rows affected by the final delete on
        /// <c>antibioticcoding</c>.
        /// </returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var sql = @"delete from listitem where id = @Id";
            await connect.ExecuteAsync(sql, new { Id = int.Parse(args[0]) });

            sql = @"delete from antibioticcoding where codingid = @Id";
            return await connect.ExecuteAsync(sql, new { Id = int.Parse(args[0]) });
        }
    }
}
