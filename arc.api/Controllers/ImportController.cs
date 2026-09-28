//using arc.app.Common;
//using arc.app.Import;
//using arc.app.Security;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;

//namespace arc.api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImportController : ControllerBase
//    {
//        private readonly ILogWriter _logger;
//        private readonly ITokenHandler _tokenHandler;
//        private readonly IImportHandler _importHandler;

//        public ImportController(ILogWriter logger, ITokenHandler tokenHandler, IImportHandler importHandler)
//        {
//            _logger = logger;
//            _tokenHandler = tokenHandler;
//            _importHandler = importHandler;
//        }

//        [Route("import")]
//        [HttpPost]
//        public async Task<ActionResult> Import()
//        {
//            try
//            {
//                var token = _tokenHandler.GetTokenInfo(User);
//                if (string.IsNullOrEmpty(token.Username)) { return Unauthorized(); }

//                var contents = "";
//                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
//                {
//                    contents = await reader.ReadToEndAsync();
//                }

//                var result = await _importHandler.RunImport(contents);

//                return Ok(result);

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception while running import {ex}", "ImportController", "Import");
//                return StatusCode(500, "A problem happened while handling your request.");
//            }
//        }
//    }
//}
