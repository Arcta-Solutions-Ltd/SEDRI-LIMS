using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    public class GetNextAccessionNumberQuery : IQueryReturningString
    {
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var prefix = queryFilters.Parameters.Where(p => p.Key.ToLower() == "prefix").First();
            var seedMask = queryFilters.Parameters.Where(p => p.Key.ToLower() == "seedmask").First();
            var numberLength = queryFilters.Parameters.Where(p => p.Key.ToLower() == "numberlength").First();
            var category = queryFilters.Parameters.Where(p => p.Key.ToLower() == "category").First();

            var sql = "select * from AccessionNumber where Prefix = @Prefix and category = @Category";
            var prefixResult = await connect.QueryFirstOrDefaultAsync<AccessionNumberData>(sql, new { Prefix = prefix.Value, Category = category.Value});
            var seedMaskResult = await connect.QueryFirstOrDefaultAsync<AccessionNumberData>(sql, new { Prefix = seedMask.Value, Category = category.Value });

            var prefixCounter = prefixResult != null ? prefixResult.Counter : 0;
            var seedMaskCounter = seedMaskResult != null ? seedMaskResult.Counter : 0;

            var newCount = Math.Max(prefixCounter, seedMaskCounter) + 1;

            var count = "";
            if (seedMaskResult != null)
            {
                sql = "update AccessionNumber set Counter = @Counter where Prefix = @Prefix and category = @Category";
                await connect.ExecuteAsync(sql, new { Counter = newCount, Prefix = seedMask.Value, Category = category.Value });
            }
            else
            {
                sql = "insert into AccessionNumber(Prefix, Counter, Category) Values(@Prefix, @Counter, @Category)";
                await connect.ExecuteAsync(sql, new { Counter = newCount, Prefix = seedMask.Value, Category = category.Value });
            }
            var counterFormat = new string('0', int.Parse(numberLength.Value));
            count = newCount.ToString(counterFormat);
            return prefix.Value == "<:sequence:>" ? count : prefix.Value + count;
        }
    }
}
