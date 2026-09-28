using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IStateRepository
    {
        Task<string> GetStateAsync(string id, string table, string field);
    }
}
