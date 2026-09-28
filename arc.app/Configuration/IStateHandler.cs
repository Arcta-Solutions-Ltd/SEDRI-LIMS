using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IStateHandler
    {
        Task<string> GetCurrentStateFromDatabaseAsync(string id, string table, string field);
    }
}
