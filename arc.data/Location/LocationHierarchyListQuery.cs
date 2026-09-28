using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Location
{
    internal class LocationHierarchyListQuery : IQueryReturningString
    {
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var locationId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "locationid");

            if (locationId.Count() > 0)
            {
                var locationQuery = new LocationIdListFromLocationIdQuery();
                var location = locationId.First();
                if  (! string.IsNullOrEmpty(location.Value) )
                {
                    var locationIds = await locationQuery.Execute(connect, int.Parse(location.Value));
                    return string.Join(",", locationIds);
                } else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
