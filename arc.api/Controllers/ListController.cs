using arc.app.Common;
using arc.app.Security;
using arc.common.Models;
using arc.common.Models.Lists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// Handles API requests related to getting list information.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ListController"/> class.
/// </remarks>
/// <param name="logWriter">The logger to log information.</param>
/// <param name="controllerUtils">The controller utilities.</param>
/// <param name="tokenHandler">The token handler for authorization.</param>
/// <param name="listHandler">The handler to manage lists.</param>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class ListController(ILogWriter logWriter, IControllerUtils controllerUtils, ITokenHandler tokenHandler, IHandleList listHandler) : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IHandleList _listHandler = listHandler ?? throw new ArgumentNullException(nameof(listHandler));

    /// <summary>
    /// Retrieves the contents of the list(s) requested.
    /// </summary>
    /// <returns>The contents of a list(s) in the format understood by the UI</returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("get")]
    [HttpPost]
    public async Task<ActionResult> GetAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, listModel) = await AuthorizeAndGetContentAsync<List<DynamicListModel>>();
            if (authResult != null) { return authResult; }

            var result = await _listHandler.HandleAsync(listModel, token);
            return Ok(result);
        }, nameof(GetAsync));
    }
}
