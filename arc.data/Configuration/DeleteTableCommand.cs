using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Command to soft-delete a list (marks the record as deleted instead of removing it).
    /// </summary>
    internal class DeleteTableCommand : ICommand
    {
        /// <summary>
        /// Executes the delete command by setting the Deleted flag to true on the specified list.
        /// </summary>
        /// <param name="connect">An open NpgsqlConnection to the database.</param>
        /// <param name="args">Expected: args[0] = list Id (string representation of an integer).</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = "Update List set Deleted = true Where Id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
