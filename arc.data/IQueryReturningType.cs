using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface IQueryReturningType<T>
    {
        Task<T> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters);
    }
}
