using arc.app.Common;
using arc.domain.Security.User;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Command that updates an existing user in the system.
/// </summary>
internal class EditUserCommand : ICommandWithTypeReturningInteger<User>
{
    /// <summary>
    /// Executes the command to update user details in the Users table and refreshes
    /// related associations in the UserRole, LaboratoryUser, and OrganisationUser tables.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute SQL commands.
    /// </param>
    /// <param name="command">
    /// A <see cref="User"/> object containing the updated user details and identifiers.
    /// </param>
    /// <param name="logWriter">
    /// An instance of <see cref="ILogWriter"/> used to log the execution details and any errors.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the Id of the updated user.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, User command, ILogWriter logWriter)
    {
        var sql = @"update Users set FirstName = @FirstName, LastName = @LastName, Email = @Email, Enabled = @Enabled,  
                                    LastModifiedDate = now() where Id = @Id";
        await connect.ExecuteAsync(sql, command);

        sql = "delete from UserRole Where UserId = @UserId";
        await connect.ExecuteAsync(sql, new { UserId = command.Id });

        var roleArray = command.Roles.Split(",");
        foreach (var role in roleArray)
        {
            sql = @"insert into UserRole(UserId, RoleId, LastModifiedDate) values(@UserId, @RoleId, now())";
            await connect.ExecuteAsync(sql, new { UserId = command.Id, RoleId = int.Parse(role) });
        }

        sql = "delete from LaboratoryUser Where UserId = @UserId";
        await connect.ExecuteAsync(sql, new { UserId = command.Id });

        if (!string.IsNullOrEmpty(command.LaboratoryId))
        {
            var labArray = command.LaboratoryId.Split(",");
            foreach (var lab in labArray)
            {
                sql = @"insert into LaboratoryUser(LaboratoryId, UserId) values(@LaboratoryId,@UserId )";
                await connect.ExecuteAsync(sql, new { UserId = command.Id, LaboratoryId = int.Parse(lab) });
            }
        }

        sql = "delete from OrganisationUser Where UserId = @UserId";
        await connect.ExecuteAsync(sql, new { UserId = command.Id });

        if (!string.IsNullOrEmpty(command.OrganisationId))
        {
            var orgArray = command.OrganisationId.Split(",");
            foreach (var org in orgArray)
            {
                sql = @"insert into OrganisationUser(OrganisationId, UserId, lastmodifieddate) values(@OrganisationId,@UserId, now() )";
                await connect.ExecuteAsync(sql, new { UserId = command.Id, OrganisationId = int.Parse(org) });
            }
        }

        return command.Id;
    }
}
