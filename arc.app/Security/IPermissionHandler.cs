using arc.domain.Security.Permission;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface IPermissionHandler
    {
        Task<Role> GetPermissionsAsync(string userName);
    }
}
