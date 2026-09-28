using arc.common.Models.User;
using arc.domain;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface IUserRepository
    {
        Task<string> GetListAsync(QueryFilterConfig queryFilters);
        Task AddUserAsync(User user);
        Task EditUserAsync(User user);
        Task<string> GetSingleUserForUserListAsync(QueryFilterConfig queryFilters);
        Task<string> GetUserByIdAsync(QueryFilterConfig queryFilters);
        Task ChangePasswordAsync(User user);
        Task DeleteUserAsync(string id);
        Task<Preference> PreferenceByIdAsync(QueryFilterConfig queryFilters);
        Task EditUserPreferenceAsync(Preference preference);
        Task<PreferenceConfigModel> PreferenceByUsernameAsync(string userName);

        /// <summary>
        /// Retrieves the MoreData JSON for a user by Id.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>The MoreData JSON string, or null if not set.</returns>
        Task<string> GetUserMoreDataByIdAsync(int userId);
    }
}
