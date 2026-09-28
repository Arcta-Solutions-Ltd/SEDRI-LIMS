using arc.app.Security;
using arc.common.Models.User;
using arc.domain;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Repository that handles user-related data operations such as querying user lists,
/// retrieving individual users, and managing user commands (add, edit, delete, etc.).
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ISqlQuery _sqlQuery;
    private readonly ISqlCommand _sqlCommand;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="sqlQuery">The SQL query executor.</param>
    /// <param name="sqlCommand">The SQL command executor.</param>
    public UserRepository(ISqlQuery sqlQuery, ISqlCommand sqlCommand)
    {
        _sqlQuery = sqlQuery;
        _sqlCommand = sqlCommand;
    }

    /// <summary>
    /// Retrieves the user list as a string based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The configuration for filtering the query.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the user list as a string.
    /// </returns>
    public async Task<string> GetListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningStringAsync(new UserListQuery(), "Get User List", queryFilters);
    }

    /// <summary>
    /// Retrieves a single user record for the user list as a string based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The configuration for filtering the query.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the user data as a string.
    /// </returns>
    public async Task<string> GetSingleUserForUserListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningStringAsync(new SingleUserForUserListQuery(), "Get Single User For User List", queryFilters);
    }

    /// <summary>
    /// Retrieves a user by Id as a string based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The configuration for filtering the query, which includes the user Id.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the user data as a string.
    /// </returns>
    public async Task<string> GetUserByIdAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningStringAsync(new UserByIdQuery(), "Get User By Id", queryFilters);
    }

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="user">A <see cref="User"/> object containing the user details to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddUserAsync(User user)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new AddUserCommand(), "Add User", user);
    }

    /// <summary>
    /// Edits an existing user's details.
    /// </summary>
    /// <param name="user">A <see cref="User"/> object containing the updated user details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task EditUserAsync(User user)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new EditUserCommand(), "Edit User", user);
    }

    /// <summary>
    /// Updates the preferences of a user.
    /// </summary>
    /// <param name="preference">A <see cref="Preference"/> object containing the updated user preferences.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task EditUserPreferenceAsync(Preference preference)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new EditUserPreferenceCommand(), "Edit User Preference", preference);
    }

    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    /// <param name="user">A <see cref="User"/> object containing the new password details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ChangePasswordAsync(User user)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new ChangePasswordCommand(), "Change Password", user);
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="id">The identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteUserAsync(string id)
    {
        await _sqlCommand.CarryOutCommandAsync(new DeleteUserCommand(), "Delete User", id);
    }

    /// <summary>
    /// Retrieves the user preference by user Id.
    /// </summary>
    /// <param name="queryFilters">The configuration for filtering the query, which includes the user Id.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Preference"/> object.
    /// </returns>
    public async Task<Preference> PreferenceByIdAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new PreferenceByIdQuery(), "Get User Preference By Id", queryFilters);
    }

    /// <summary>
    /// Retrieves the user preference configuration by username.
    /// </summary>
    /// <param name="userName">The username for which to retrieve the preference configuration.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="PreferenceConfigModel"/> object.
    /// </returns>
    public async Task<PreferenceConfigModel> PreferenceByUsernameAsync(string userName)
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("username", userName);
        return await _sqlQuery.QueryReturningTypeAsync(new PreferenceByUsernameQuery(), "Get User Preference By Username", queryFilter);
    }

    /// <summary>
    /// Retrieves the MoreData JSON for a user by Id.
    /// </summary>
    /// <param name="userId">The user Id.</param>
    /// <returns>The MoreData JSON string, or null if not set.</returns>
    public async Task<string> GetUserMoreDataByIdAsync(int userId)
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddInteger("id", userId);
        return await _sqlQuery.QueryReturningStringAsync(new GetUserMoreDataByIdQuery(), "Get User MoreData By Id", queryFilter);
    }
}
