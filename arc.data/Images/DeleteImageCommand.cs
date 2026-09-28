using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a command to soft delete an image record.
/// </summary>
internal class DeleteImageCommand : ICommandReturningInteger
{
    /// <summary>
    /// Executes the command asynchronously to soft delete an image record in the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="args">The image ID to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of rows affected.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        if (args == null || args.Length == 0 || !int.TryParse(args[0], out var id))
        {
            return 0;
        }

        var sql = @"delete from images where id = @Id";

        return await connect.ExecuteAsync(sql, new { Id = id });
    }
}
