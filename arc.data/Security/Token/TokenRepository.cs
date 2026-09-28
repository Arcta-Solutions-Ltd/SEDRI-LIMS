using arc.common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.data.Security.Token;

/// <summary>
/// Repository responsible for retrieving token information based on the username.
/// </summary>
public class TokenRepository : ITokenRepository
{
    private readonly ISqlQuery _sqlQuery;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRepository"/> class.
    /// </summary>
    /// <param name="options">The options monitor providing data connection settings.</param>
    /// <param name="logger">The logger for recording errors and other log messages.</param>
    public TokenRepository(ISqlQuery sqlQuery)
    {
        _sqlQuery = sqlQuery;
    }

    /// <summary>
    /// Retrieves token information based on the specified email address and lab ID.
    /// </summary>
    /// <param name="emailAddress">The email address used for token retrieval.</param>
    /// <param name="labId">The lab identifier associated with the token query.</param>
    /// <returns>A Task resolving to a TokenInfoModel containing token details.</returns>
    public async Task<TokenInfoModel> GetTokenInfoByEmailAddress(string emailAddress)
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("emailaddress", emailAddress);

        return await _sqlQuery.QueryReturningTypeAsync(new TokenByUsernameQuery(), "Get security token by username", queryFilter);
    }


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
    public async Task<TokenInfoModel> GetTokenInfoWithUserNameAsync(string userName, string labId, string orgId)
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("username", userName)
                   .AddString("labid", labId)
                   .AddString("orgid", orgId);

        return await _sqlQuery.QueryReturningTypeAsync(new TokenByUsernameQuery(), "Get security token by username", queryFilter);
    }
}
