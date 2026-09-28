using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface IQueryReturningInteger
    {
        Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters);
    }
}
