using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IFormQueryHandler
    {
        Task<string> HandleAsync(string form, string id);
    }
}
