using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Command that deletes a user from the system along with all associated records 
/// from LaboratoryUser, OrganisationUser, and UserRole tables.
/// </summary>
internal class DeleteUserCommand : ICommand
{
    /// <summary>
    /// Executes the delete commands to remove the user and related records from the database.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute the SQL commands.
    /// </param>
    /// <param name="args">
    /// A string array where the first element is the user's ID that identifies which user to delete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, with the task result containing the number 
    /// of rows affected by the final deletion from the Users table.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);
        var sql = @"delete from LaboratoryUser where UserId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from OrganisationUser where UserId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from UserRole where UserId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from Users where Id = @Id";
        return await connect.ExecuteAsync(sql, new { Id = id });
    }
}
