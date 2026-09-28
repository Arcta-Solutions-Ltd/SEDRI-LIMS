using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface ISetupRespository
    {
        Task SaveSetup(string message);
    }
}
