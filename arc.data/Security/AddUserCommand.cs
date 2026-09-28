using arc.app.Common;
using arc.domain.Security.User;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Command that adds a new user to the system, including associated roles,
/// laboratories, and organisations.
/// </summary>
internal class AddUserCommand : ICommandWithTypeReturningInteger<User>
{
    /// <summary>
    /// Executes the command to insert a new user into the Users table and creates
    /// associations in the UserRole, LaboratoryUser, and OrganisationUser tables.
    /// </summary>
    /// <param name="connect">An open NpgsqlConnection used to execute the SQL queries.</param>
    /// <param name="command">A User object containing the user's details to be inserted.</param>
    /// <param name="logWriter">A logger to record the execution details and any errors.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the Id
    /// of the newly inserted user.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, User command, ILogWriter logWriter)
    {
        var sql = @"insert into Users(UserName, FirstName, LastName, Password, Email, Enabled, LastModifiedDate)  
                                    Values(@UserName, @FirstName, @LastName, @Password, @Email, @Enabled, now()) returning Id";
        var userId = connect.Query<int>(sql, command).Single();

        var roleArray = command.Roles.Split(",");
        foreach (var role in roleArray)
        {
            sql = @"insert into UserRole(UserId, RoleId, LastModifiedDate) values(@UserId, @RoleId, now())";
            await connect.ExecuteAsync(sql, new { UserId = userId, RoleId = int.Parse(role) });
        }

        if (!string.IsNullOrEmpty(command.LaboratoryId))
        {
            var labArray = command.LaboratoryId.Split(",");
            foreach (var lab in labArray)
            {
                sql = @"insert into LaboratoryUser(LaboratoryId, UserId) values(@LaboratoryId, @UserId)";
                await connect.ExecuteAsync(sql, new { UserId = userId, LaboratoryId = int.Parse(lab) });
            }
        }

        if (!string.IsNullOrEmpty(command.OrganisationId))
        {
            var orgArray = command.OrganisationId.Split(",");
            foreach (var org in orgArray)
            {
                sql = @"insert into OrganisationUser(OrganisationId, UserId, LastModifiedDate) values(@OrganisationId, @UserId, now())";
                await connect.ExecuteAsync(sql, new { UserId = userId, OrganisationId = int.Parse(org) });
            }
        }

        return userId;
    }
}
