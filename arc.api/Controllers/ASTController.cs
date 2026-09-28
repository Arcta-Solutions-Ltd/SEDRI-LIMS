using arc.app.AST;
using arc.app.Common;
using arc.app.Security;
using arc.common.Models.AST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace arc.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ASTController(ILogWriter logWriter, IControllerUtils controllerUtils, ITokenHandler tokenHandler, IASTHandler astHandler, ILogger<ASTController> logger) : BaseController(logWriter, controllerUtils, tokenHandler)
    {
        private readonly IASTHandler _ASTHandler = astHandler;
        private readonly ILogger<ASTController> _logger = logger;

        /// <summary>
        /// Discovers special-consideration embedded rows from breakpoints (id-based matching) and resolves susceptibilities
        /// for one AST line and its embeds when measurement allows. Expert rules are refreshed via <c>getexpertrulesforpage</c>.
        /// </summary>
        [Route("getsusceptibility")]
        [HttpPost]
        public async Task<ActionResult> GetSusceptibilityAsync()
        {
            var (authResult, token, content) = await AuthorizeAndGetContentAsync<ASTRowModel>();
            if (authResult != null) { return authResult; }

            var result = await _ASTHandler.GetSusceptibilitiesAndExpertRules(content);

            return Ok(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// Returns expert rule groups and flat result rows for the AST page using the supplied disk/MIC state (multi-line evaluation).
        /// </summary>
        [Route("getexpertrulesforpage")]
        [HttpPost]
        public async Task<ActionResult> GetExpertRulesForPageAsync()
        {
            var (authResult, token, content) = await AuthorizeAndGetContentAsync<ExpertRulesForPageRequest>();
            if (authResult != null) { return authResult; }

            var sw = Stopwatch.StartNew();
            var result = await _ASTHandler.GetExpertRulesForPageAsync(content);
            sw.Stop();

            _logger.LogInformation(
                "AST getexpertrulesforpage: CultureId={CultureId}, ExpertRuleGroupCount={GroupCount}, CommentAlertCount={CommentCount}, ElapsedMs={ElapsedMs:F1}",
                content.CultureId,
                result.ExpertRuleGroups?.Count ?? 0,
                result.ExpertRuleCommentAlerts?.Count ?? 0,
                sw.Elapsed.TotalMilliseconds);

            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}
