using System.Threading.Tasks;

namespace arc.common
{
    public interface ILoginRepository
    {
        Task<string> GetSingleValueAsync(string field, string value);
        void AddConfiguration(string collection);
    }
}
