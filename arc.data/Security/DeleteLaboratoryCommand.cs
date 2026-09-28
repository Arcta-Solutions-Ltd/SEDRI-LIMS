using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Command for deleting a laboratory record.
/// </summary>
internal class DeleteLaboratoryCommand : ICommand
{
    /// <summary>
    /// Executes the delete command for a laboratory record asynchronously.
    /// </summary>
    /// <param name="connect">The database connection to execute the command.</param>
    /// <param name="args">Parameters containing the ID of the laboratory to delete.</param>
    /// <returns>The number of affected rows as an integer.</returns>
    /// <remarks>
    /// This method interacts with the database to delete the specified laboratory record 
    /// and its associated configurations. It performs the following operations:
    /// 1. Deletes the laboratory record from the "laboratory" table based on the given ID.
    /// 2. Deletes the corresponding configurations from the "laboratoryconfigs" table where the laboratory ID matches.
    /// The method ensures both the primary record and associated data are removed, returning the 
    /// number of rows affected by the final operation.
    /// </remarks>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var sql = @"delete from laboratory where id = @Id";
        await connect.ExecuteAsync(sql, new { id = int.Parse(args[0]) });

        sql = @"delete from laboratoryconfigs where laboratoryid = @Id";
        return await connect.ExecuteAsync(sql, new { id = int.Parse(args[0]) });
    }
}
