using arc.common.Models;
using arc.common.Models.User;
using System.Security.Claims;

namespace arc.app.Security
{
    public class TokenHandler : ITokenHandler
    {
        private readonly TokenInfoModel _tokenInfoModel;

        public TokenHandler(TokenInfoModel tokenInfoModel)
        {
            _tokenInfoModel = tokenInfoModel;
        }

        public TokenInfoModel GetTokenInfo(ClaimsPrincipal user)
        {
            var token = new TokenInfoModel();
            token.Username = user.FindFirst(ClaimTypes.UserData)?.Value;
            token.LanguageId = user.FindFirst("LanguageId")?.Value;
            token.LaboratoryId = user.FindFirst("LaboratoryId")?.Value;
            token.OrganisationId = user.FindFirst("OrganisationId")?.Value;
            token.Tags = user.FindFirst("Tags")?.Value;
            token.Id = user.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(token.LanguageId)) { token.LanguageId = "669"; } 

            _tokenInfoModel.Username = token.Username;

            return token;
        }
    }
}
