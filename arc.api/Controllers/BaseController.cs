using arc.app.Common;
using arc.app.Security;
using arc.common.Models;
using arc.common.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace arc.api.Controllers
{
    /// <summary>
    /// Abstract base controller class that provides common functionality for derived controllers.
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected string ContentsAsString {get; set;}

        /// <summary>
        /// The token handler used for managing token information.
        /// </summary>
        protected readonly ITokenHandler _tokenHandler;

        /// <summary>
        /// Utility functions for the controller.
        /// </summary>
        protected readonly IControllerUtils _controllerUtils;

        /// <summary>
        /// Logger for writing log information.
        /// </summary>
        protected readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="controllerUtils">The controller utilities.</param>
        /// <param name="tokenHandler">The token handler.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        protected BaseController(ILogWriter logWriter, IControllerUtils controllerUtils, ITokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
            _controllerUtils = controllerUtils ?? throw new ArgumentNullException(nameof(controllerUtils));
            _logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Initializes and checks authorization asynchronously.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> representing the result of the authorization check.</returns>
        protected async Task<ActionResult> InitializeAndCheckAuthorizationAsync()
        {
            await _controllerUtils.InitialiseAsync(Request.Body);
            if (!await _controllerUtils.CheckAuthorisationAsync(User))
            {
                _logWriter.LogInfo("Unauthorised access request detected", GetType().Name, nameof(InitializeAndCheckAuthorizationAsync));
                return Unauthorized();
            }
            return null;
        }

        /// <summary>
        /// Handles the authorization and retrieves the contents.
        /// </summary>
        /// <typeparam name="T">The type to which the content will be deserialized.</typeparam>
        /// <returns>A tuple containing the authorization result and the deserialized content.</returns>
        protected async Task<(ActionResult, TokenInfoModel, T)> AuthorizeAndGetContentAsync<T>() where T : class
        {
            var token = _tokenHandler.GetTokenInfo(User);

            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null)
            {
                return (authResult, token, null);
            }

            ContentsAsString = _controllerUtils.GetContents();
            var content = JsonConvert.DeserializeObject<T>(ContentsAsString);
            return (null, token, content);
        }

        /// <summary>
        /// Executes an action with performance monitoring and standardized logging.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="operationName">The name of the operation for logging purposes.</param>
        /// <returns>An ActionResult representing the result of the operation.</returns>
        protected async Task<ActionResult> ExecuteWithMonitoringAsync(Func<Task<ActionResult>> action, string operationName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logWriter.LogInfo($"Starting {operationName}", GetType().Name, operationName);
                var result = await action();
                stopwatch.Stop();
                _logWriter.LogInfo($"Completed {operationName} in {stopwatch.ElapsedMilliseconds}ms", GetType().Name, operationName);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                try
                {
                    _logWriter.LogError($"Exception in {operationName} after {stopwatch.ElapsedMilliseconds}ms: {ex}", GetType().Name, operationName);
                }
                catch
                {
                    // Logging must not prevent the error response from being sent.
                }

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Serializes an object and translates it using the language handler.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize and translate.</param>
        /// <param name="languageId">The language ID for translation.</param>
        /// <param name="languageHandler">The language handler for translation.</param>
        /// <returns>A translated JSON string.</returns>
        protected async Task<string> SerializeAndTranslateAsync<T>(T obj, string languageId, ILanguageHandler languageHandler)
        {
            var serialized = ArcJson.Serialize(obj).Replace("'", "\"");
            return await languageHandler.TranslateAsync(serialized, languageId);
        }

        /// <summary>
        /// Validates that a required string parameter is not null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter for error messages.</param>
        /// <param name="methodName">The name of the calling method for logging.</param>
        /// <returns>BadRequest result if validation fails, null if validation passes.</returns>
        protected ActionResult ValidateRequiredParameter(string value, string parameterName, string methodName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logWriter.LogInfo($"Required parameter '{parameterName}' is null or empty", GetType().Name, methodName);
                return BadRequest($"Required parameter '{parameterName}' cannot be null or empty.");
            }
            return null;
        }

        /// <summary>
        /// Validates that a required object parameter is not null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter for error messages.</param>
        /// <param name="methodName">The name of the calling method for logging.</param>
        /// <returns>BadRequest result if validation fails, null if validation passes.</returns>
        protected ActionResult ValidateRequiredParameter(object value, string parameterName, string methodName)
        {
            if (value == null)
            {
                _logWriter.LogInfo($"Required parameter '{parameterName}' is null", GetType().Name, methodName);
                return BadRequest($"Required parameter '{parameterName}' cannot be null.");
            }
            return null;
        }

        /// <summary>
        /// Creates a standardized success response.
        /// </summary>
        /// <typeparam name="T">The type of the response data.</typeparam>
        /// <param name="data">The response data.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="message">The success message.</param>
        /// <returns>An OkResult containing the data.</returns>
        protected ActionResult CreateSuccessResponse<T>(T data, string operationName, string message)
        {
            _logWriter.LogInfo(message, GetType().Name, operationName);
            return Ok(data);
        }

        /// <summary>
        /// Creates a standardized error response.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="statusCode">The HTTP status code (default: 400).</param>
        /// <returns>A StatusCodeResult with the specified status code and message.</returns>
        protected ActionResult CreateErrorResponse(string message, string operationName, int statusCode = 400)
        {
            _logWriter.LogError(message, GetType().Name, operationName);
            return StatusCode(statusCode, message);
        }
    }

}
