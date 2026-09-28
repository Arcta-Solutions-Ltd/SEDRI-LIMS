using System.Threading.Tasks;

namespace arc.app.Roles
{
    public interface IAddRoleDataHandler
    {
        Task<string> GetInitialDataAsync();
    }
}
