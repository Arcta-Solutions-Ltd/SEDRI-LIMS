using arc.common;
using arc.common.Models;
using arc.domain.Security.User;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.identity;

/// <summary>
/// Handles login commands by verifying user credentials and retrieving token information.
/// </summary>

public class LoginCommandHandler(ILoginRepository genericRepository, ITokenRepository tokenRepository, IPasswordHasher<User> passwordHasher) : ILoginCommandHandler
{
    private readonly ILoginRepository _genericRepository = genericRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    /// <summary>
    /// Gets the error message generated during login failures.
    /// </summary>
    public string ErrorMessage { get; private set; }

    /// <summary>
    /// Handles the login operation. Verifies that the user exists, is enabled,
    /// and that the provided password matches the stored password hash.
    /// </summary>
    /// <param name="command">The login command containing user credentials.</param>
    /// <param name="errorText">The error text object containing various error messages.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task returns true if login succeeds;
    /// otherwise, false and sets the appropriate error message.
    /// </returns>
    public async Task<bool> HandleAsync(LoginCommand command, AuthenticationErrorText errorText)
    {
        _genericRepository.AddConfiguration("users");
        var result = await _genericRepository.GetSingleValueAsync("Username", command.UserName);
        var user = JsonConvert.DeserializeObject<User>(result);

        if (user == null)
        {
            ErrorMessage = errorText.UnrecognisedUser;
            return false;
        }

        if (!user.IsEnabled())
        {
            ErrorMessage = errorText.UserInvalid;
            return false;
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, command.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            return true;
        }

        ErrorMessage = errorText.PasswordInvalid;
        return false;
    }


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
    public async Task<TokenInfoModel> GetTokenInfoAsync(string userName, string labid = "", string orgid = "")
    {
        return await _tokenRepository.GetTokenInfoWithUserNameAsync(userName, labid, orgid);
    }

    /// <summary>
    /// Retrieves token information based on the given email address.
    /// </summary>
    /// <param name="emailAddress">The email address used to locate token information.</param>
    /// <returns>A Task that resolves to a TokenInfoModel containing token details.</returns>
    public async Task<TokenInfoModel> GetTokenInfoByEmailAddressAsync(string emailAddress)
    {
        return await _tokenRepository.GetTokenInfoByEmailAddress(emailAddress);
    }
}
