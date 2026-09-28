using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface IQuerySendingAndReturningType<T>
    {
        Task<T> ExecuteAsync(NpgsqlConnection connect, T Entity, QueryFilterConfig queryFilters);
    }
}
