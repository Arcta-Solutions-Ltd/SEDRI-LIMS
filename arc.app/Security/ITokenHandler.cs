using arc.common.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface ITokenHandler
    {
        TokenInfoModel GetTokenInfo(ClaimsPrincipal user);
    }
}
