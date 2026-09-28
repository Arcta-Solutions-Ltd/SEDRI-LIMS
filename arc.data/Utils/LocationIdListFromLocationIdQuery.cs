using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    internal class LocationIdListFromLocationIdQuery
    {
        public async Task<List<int>> Execute(NpgsqlConnection connect, int locationId)
        {
            var returnIds = new List<int>();
            returnIds.Add(locationId);

            var sql = @"select * from location where parentlocationid = @Id";

            var results = await connect.QueryAsync<int>(sql, new { Id = locationId });

            if (results.Count() > 0)
            {
                foreach (var result in results)
                {
                    var newIds = await new  LocationIdListFromLocationIdQuery().Execute(connect, result);
                    returnIds.AddRange(newIds);
                }
            }

            return returnIds;
        }
    }
}
