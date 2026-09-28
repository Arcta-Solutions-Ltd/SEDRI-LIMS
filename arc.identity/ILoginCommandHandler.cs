using arc.common.Models;
using System.Threading.Tasks;

namespace arc.identity;

/// <summary>
/// Defines an interface for handling login commands and retrieving associated token information.
/// </summary>


public interface ILoginCommandHandler
{
    /// <summary>
    /// Processes the login request asynchronously.
    /// </summary>
    /// <param name="command">A login command containing user credentials.</param>
    /// <param name="errorText">Error text definitions used for authentication errors.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is true if login is successful; otherwise, false.
    /// </returns>
    Task<bool> HandleAsync(LoginCommand command, AuthenticationErrorText errorText);


    /// <summary>
    /// Retrieves token information for a user based on username, laboratory ID, and organization ID.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="labid">The laboratory ID associated with the user (optional, defaults to an empty string).</param>
    /// <param name="orgid">The organization ID associated with the user (optional, defaults to an empty string).</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning a <see cref="TokenInfoModel"/> 
    /// containing the user's security token details.
    /// </returns>
    Task<TokenInfoModel> GetTokenInfoAsync(string userName, string labId, string orgId);

    /// <summary>
    /// Retrieves token information based on the given email address.
    /// </summary>
    /// <param name="emailAddress">The email address used to locate token information.</param>
    /// <returns>A Task that resolves to a TokenInfoModel containing token details.</returns>
    Task<TokenInfoModel> GetTokenInfoByEmailAddressAsync(string userName);
    public string ErrorMessage { get; }

}
