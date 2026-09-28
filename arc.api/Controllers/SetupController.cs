using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.Security;
using arc.common.Models.Security;
using arc.common.Options;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace arc.api.Controllers;

[Route("api/setup")]
[ApiController]
[Produces("application/json")]
public class SetupController : BaseController
{
    private readonly ILogger<SetupController> _logger;
    private readonly ISetupHandler _handler;
    private readonly IHandleQuery _queryHandler;
    private readonly IQueryFactory _queryFactory;
    private readonly ILanguageHandler _languageHandler;
    private readonly IOptionsMonitor<AuthenticationOptions> _authenticationOptions;
    private readonly IConfiguration _configuration;

    public SetupController(ILogger<SetupController> logger, ISetupHandler handler, IHandleQuery queryHandler, IQueryFactory queryFactory, ILanguageHandler languageHandler, IOptionsMonitor<AuthenticationOptions> authenticationOptions,
        ILogWriter logWriter, IControllerUtils controllerUtils, ITokenHandler tokenHandler, IConfiguration configuration)
        : base(logWriter, controllerUtils, tokenHandler)
    {
        _logger = logger;
        _handler = handler;
        _queryHandler = queryHandler;
        _queryFactory = queryFactory;
        _languageHandler = languageHandler;
        _authenticationOptions = authenticationOptions;
        _configuration = configuration;
    }

    [Route("setup")]
    [HttpPost()]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SetupAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            await _controllerUtils.InitialiseAsync(Request.Body);
            var contents = _controllerUtils.GetContents();

            await _handler.Handle(contents);

            var validationMessage = _handler.GetValidationMessage();

            if (string.IsNullOrEmpty(validationMessage))
            {
                return Ok();
            }

            return BadRequest(validationMessage);
        }, nameof(SetupAsync));
    }


    [Route("startupcheck")]
    [HttpGet]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(Boolean), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> StartupCheckAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var parameters = "{ Name: 'UserCount' }";

            var queryFilters = JsonConvert.DeserializeObject<QueryFilterConfig>(parameters);
            var queryData = _queryFactory.GetQuery(queryFilters.Name);
            var recordCount = await _queryHandler.HandleAsync(parameters, queryFilters, queryData);

            var startUpState = new StartUpCheckModel()
            {
                ConfigureSystem = recordCount == "0" ? "Yes" : "No",
                UsernameText = "Username",
                PasswordText = "Password",
                EstablishingText = "Establishing",
                AuthenticatingText = "Authenticating",
                LoginText = "Login",
                VersionText = "Version",
                LocalLoginEnabled = _authenticationOptions.CurrentValue.Local.Enabled,
                AzureAdLoginEnabled = _authenticationOptions.CurrentValue.AzureAd.Enabled
            };

            if (_configuration.GetValue<bool>("IsEvaluationSystem"))
            {
                var evaluationLoginTextLines = new List<string>();
                for (var i = 1; i <= 3; i++)
                {
                    evaluationLoginTextLines.Add(await _languageHandler.TranslateAsync($"@FrontendLoginEvaluationDisclaimerP{i}@", "669"));
                }
                startUpState.WarningTextLines = evaluationLoginTextLines;
            }

            startUpState.UsernameText = await _languageHandler.TranslateAsync("@GenUseB@", "669"); // Default language.
            startUpState.PasswordText = await _languageHandler.TranslateAsync("@UsePasA@", "669");
            startUpState.EstablishingText = await _languageHandler.TranslateAsync("@StaEst@", "669");
            startUpState.AuthenticatingText = await _languageHandler.TranslateAsync("@StaAut@", "669");
            startUpState.LoginText = await _languageHandler.TranslateAsync("@GenLogC@", "669");
            startUpState.VersionText = await _languageHandler.TranslateAsync("@Ver@", "669");

            var result = JsonConvert.SerializeObject(startUpState);
            return Content(result ?? "{}", "application/json");
        }, nameof(StartupCheckAsync));
    }
}
