using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace arc.api
{
    public interface IControllerUtils
    {
        Task InitialiseAsync(Stream body);
        Task<bool> CheckAuthorisationAsync(ClaimsPrincipal user);
        string GetContents();
    }
}
