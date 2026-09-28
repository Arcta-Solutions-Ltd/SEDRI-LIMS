using arc.common.Models;
using arc.common.Models.Security;
using System.Threading.Tasks;

namespace arc.common;

/// <summary>
/// Defines a repository interface for retrieving token information based on a username and laboratory identifier.
/// </summary>


public interface ITokenRepository
{
    /// <summary>
    /// Retrieves token information based on the specified email address and lab ID.
    /// </summary>
    /// <param name="emailAddress">The email address used for token retrieval.</param>
    /// <returns>A Task resolving to a TokenInfoModel containing token details.</returns>
    Task<TokenInfoModel> GetTokenInfoByEmailAddress(string emailAddress);

    /// <summary>
    /// Retrieves token information for a user based on username, laboratory ID, and organization ID.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="labId">The laboratory ID associated with the user.</param>
    /// <param name="orgId">The organization ID associated with the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, returning a <see cref="TokenInfoModel"/> 
    /// containing the security token details.
    /// </returns>
    Task<TokenInfoModel> GetTokenInfoWithUserNameAsync(string userName, string labId, string orgId);
}
