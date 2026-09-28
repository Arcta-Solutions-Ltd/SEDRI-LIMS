using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface ISetupHandler
    {
        Task Handle(string message);
        string GetValidationMessage();
    }
}
