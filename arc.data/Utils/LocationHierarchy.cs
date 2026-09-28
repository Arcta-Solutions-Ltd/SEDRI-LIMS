using arc.common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils
{ 
    internal class LocationHierarchy
    {
        public async Task<string> GetStringList(NpgsqlConnection connect, int locationId)
        {
            var returnLocations = new List<string>();

            var sql = @"select id from location where parentlocationid = @Id";

            var results = await connect.QueryAsync<IdModel>(sql, new { Id = locationId });

            if (results.Count() > 0)
            {
                var resultsList = results.Select(f => f.Id);
                returnLocations.Add(string.Join(",", resultsList));
                foreach (var result in resultsList)
                {
                    var newLocations = await new LocationHierarchy().GetStringList(connect, int.Parse(result));
                    returnLocations.Add(newLocations);
                }
            }

            var finalReturnList = "";
            foreach (var row in returnLocations)
            {
                if (! string.IsNullOrWhiteSpace(row))
                {
                    finalReturnList += string.IsNullOrEmpty(finalReturnList) ? row : "," + row;
                }
            }
            return finalReturnList;
        }
    }
}
