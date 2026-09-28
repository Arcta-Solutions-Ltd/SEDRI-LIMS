using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IRepository<T>
    {
        Task AddAsync(T record);
        Task<T> GetSingleAsync(string userName);
    }
}
