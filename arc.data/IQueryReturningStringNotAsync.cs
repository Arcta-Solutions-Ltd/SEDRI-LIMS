using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;

namespace arc.data
{
    public interface IQueryReturningStringNotAsync
    {
        string Execute(NpgsqlConnection connect, QueryFilterConfig queryFilters);
    }
}
