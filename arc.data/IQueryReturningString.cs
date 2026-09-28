using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data
{
    public interface IQueryReturningString
    {
        Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters);
    }
}
