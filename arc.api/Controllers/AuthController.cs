using arc.app.Common;
using arc.app.Config;
using arc.app.Security;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Options;
using arc.identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using arc.api.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// API controller for authentication-related operations.
/// </summary>
[Route("api/auth")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IOptionsMonitor<AuthenticationOptions> _authenticationOptions;
    private readonly IRegisterHandler _registerHandler;
    private readonly ILoginCommandHandler _loginCommandHandler;
    private readonly ILanguageHandler _languageHandler;
    private readonly IQueryPermissions _queryPermissions;
    private readonly ILoginThrottleService _loginThrottle;


    public AuthController(ILogger<AuthController> logger, IOptionsMonitor<AuthenticationOptions> authenticationOptions, IRegisterHandler registerHandler, ILoginCommandHandler loginCommandHandler, ILanguageHandler languageHandler,
        IHandleConfig configHandler, IQueryPermissions queryPermissions, ILoginThrottleService loginThrottle)
    {
        _logger = logger;
        _authenticationOptions = authenticationOptions;
        _registerHandler = registerHandler;
        _loginCommandHandler = loginCommandHandler;
        _languageHandler = languageHandler;
        _queryPermissions = queryPermissions;
        _loginThrottle = loginThrottle;
    }

    /// <summary>
    /// Gets a CSRF token for login protection.
    /// </summary>
    /// <param name="antiforgery">The antiforgery service.</param>
    /// <returns>A CSRF token.</returns>
    [AllowAnonymous]
    [HttpGet("csrf-token")]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    public IActionResult GetCsrfToken([FromServices] IAntiforgery antiforgery)
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="login">Login credentials provided by the user.</param>
    /// <param name="antiforgery">The antiforgery service for CSRF validation.</param>
    /// <returns>Returns an HTTP response containing the authentication result.</returns>
    [AllowAnonymous]
    [Route("login")]
    [HttpPost()]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand login, [FromServices] IAntiforgery antiforgery)
    {
        // WAPT-007: Manual CSRF validation for API controllers
        try
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
        }
        catch (Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException ex)
        {
            return BadRequest("CSRF validation failed");
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        if (_loginThrottle.IsIpRateLimited(ip))
        {
            return StatusCode((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");
        }

        if (!_authenticationOptions.CurrentValue.Local.Enabled)
        {
            return Unauthorized("Local login is not enabled");
        }

        if (string.IsNullOrWhiteSpace(login.UserName))
        {
            var usernameErrorText = await _languageHandler.TranslateAsync("@AutErr@", "669"); // Default language.
            return BadRequest(usernameErrorText);
        }
        if (string.IsNullOrWhiteSpace(login.Password))
        {
            var passwordErrorText = await _languageHandler.TranslateAsync("@AutErrA@", "669"); // Default language.
            return BadRequest(passwordErrorText);
        }

        try
        {
            var errorText = new AuthenticationErrorText
            {
                UserInvalid = await _languageHandler.TranslateAsync("@AutErrD@", "669"), // Default language.
                PasswordInvalid = await _languageHandler.TranslateAsync("@AutErrD@", "669"),
                UnrecognisedUser = await _languageHandler.TranslateAsync("@AutErrD@", "669")
            };

            var haveAccess = await _loginCommandHandler.HandleAsync(login, errorText);
            if (!haveAccess)
            {
                _loginThrottle.RecordLoginAttempt(login.UserName ?? string.Empty, ip, false);
                if (_loginThrottle.IsUserThrottled(login.UserName ?? string.Empty))
                {
                    return StatusCode((int)HttpStatusCode.TooManyRequests, "Too many failed login attempts. Please try again later.");
                }
                return Unauthorized(_loginCommandHandler.ErrorMessage);
            }

            _loginThrottle.RecordLoginAttempt(login.UserName ?? string.Empty, ip, true);
            var tokenInfo = await _loginCommandHandler.GetTokenInfoAsync(login.UserName,"","");

            tokenInfo.Tags = await _queryPermissions.GetQueryPermssionStringAsync(tokenInfo, tokenInfo.LanguageId);

            var token = CreateJwtSecurityToken(tokenInfo);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Exception while logging the user in {ex}", ex);
            return StatusCode(500, ex);
            //return StatusCode(500, "A problem happened while handling your request.");
        }
    }

    /// <summary>
    /// Switches the user's organization or laboratory based on the provided login details.
    /// Logs an informational entry with username and resolved LaboratoryId / OrganisationId (no PHI) for support on deployed systems.
    /// </summary>
    /// <param name="login">The login command containing the new laboratory or organization ID.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning an <see cref="IActionResult"/>
    /// containing the updated JWT token.
    /// </returns>
    [HttpPost("switch")]
    [HttpPost]
    public async Task<IActionResult> SwitchOrganisationOrLaboratoryAsync([FromBody] LoginCommand login)
    {
        try
        {
            login.UserName = User.FindFirst(ClaimTypes.UserData)?.Value;
            var tokenInfo = await _loginCommandHandler.GetTokenInfoAsync(login.UserName, login.LabId, login.OrgId);
            tokenInfo.Tags = await _queryPermissions.GetQueryPermssionStringAsync(tokenInfo, tokenInfo.LanguageId);
            var token = CreateJwtSecurityToken(tokenInfo);

            _logger.LogInformation(
                "User {Username} switched laboratory/organisation scope: LaboratoryId={LaboratoryId}, OrganisationId={OrganisationId}",
                login.UserName,
                tokenInfo.LaboratoryId ?? string.Empty,
                tokenInfo.OrganisationId ?? string.Empty);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error switching organisation or laboratory");
            return StatusCode(500, "A problem happened while handling your request.");
        }
    }

    /// <summary>
    /// Handles the Azure AD authentication callback and generates a JWT token.
    /// </summary>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> containing the generated token if authentication is successful,
    /// or an Unauthorized response if authentication fails.
    /// </returns>

    [HttpPost("callback")]
    [Authorize(AuthenticationSchemes = "AzureAd")]
    public async Task<IActionResult> AzureCallbackAsync()
    {
        if (!_authenticationOptions.CurrentValue.AzureAd.Enabled)
        {
            return Unauthorized("Azure login is not enabled.");
        }

        var azureClaims = User.Claims;
        var emailAddress = azureClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var objectId = azureClaims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        var givenName = azureClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        var surname = azureClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

        try
        {
            var tokenInfo = await _loginCommandHandler.GetTokenInfoByEmailAddressAsync(emailAddress);
            tokenInfo.Tags = await _queryPermissions.GetQueryPermssionStringAsync(tokenInfo, tokenInfo.LanguageId);
            var token = CreateJwtSecurityToken(tokenInfo, "azure");

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        catch (Exception)
        {
            return Unauthorized("User is not authorised to access this system.");
        }
    }


    /// <summary>
    /// Handles the registration process by reading the request body and validating the contents.
    /// </summary>
    /// <returns>
    /// Returns an <see cref="IActionResult"/>:
    /// - **200 OK** if registration is successful.
    /// - **400 Bad Request** if validation fails.
    /// - **500 Internal Server Error** if an exception occurs.
    /// </returns>

    [Route("register")]
    [HttpPost]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RegisterAsync()
    {
        try
        {
            var contents = "";
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                contents = await reader.ReadToEndAsync();
            }

            _registerHandler.Handle(contents);
            var validationMessage = _registerHandler.GetValidationMessage();

            if (string.IsNullOrEmpty(validationMessage))
            {
                return Ok();
            }
            else
            {
                return BadRequest(validationMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Exception while logging the user in {ex}", ex);
            return StatusCode(500, "A problem happened while handling your request.");
        }
    }

    /// <summary>
    /// Creates a JWT security token based on provided token information.
    /// </summary>
    /// <param name="tokenInfo">The user's token information.</param>
    /// <param name="authMode">The authentication method (default is "local").</param>
    /// <returns>A generated <see cref="JwtSecurityToken"/> containing user claims.</returns>
    private JwtSecurityToken CreateJwtSecurityToken(TokenInfoModel tokenInfo, string authMode = "local")
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.CurrentValue.Local.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var claims = new List<Claim>
        {
            new(ClaimTypes.UserData, tokenInfo.Username),
            new("LaboratoryId", tokenInfo.LaboratoryId.IsIntegerGreaterThan(0) ? tokenInfo.LaboratoryId : ""),
            new("OrganisationId", tokenInfo.OrganisationId.IsIntegerGreaterThan(0) ? tokenInfo.OrganisationId : ""),
            new("LanguageId", tokenInfo.LanguageId ?? ""),
            new("Id", tokenInfo.Id ?? ""),
            new("AuthMethod", authMode),
            new("AllowedLaboratories", tokenInfo.AllowedLaboratories ?? ""),
            new("AllowedOrganisations", tokenInfo.AllowedOrganisations ?? ""),
            new("Tags", tokenInfo.Tags ?? ""),
            new(ClaimTypes.GivenName, tokenInfo.FirstName ?? ""),
            new(ClaimTypes.Surname, tokenInfo.LastName ?? ""),
        };

        return new JwtSecurityToken(
            issuer: _authenticationOptions.CurrentValue.Local.Issuer,
            audience: _authenticationOptions.CurrentValue.Local.Audience,
            expires: DateTime.UtcNow.AddMinutes(600),
            claims: claims,
            signingCredentials: creds);
    }
}
