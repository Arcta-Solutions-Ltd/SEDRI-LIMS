using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Returns antibiotic id and name pairs for the ids supplied in the <c>ids</c> query filter (comma-separated).
/// </summary>
internal class AntibioticNamesByIdsQuery : IQueryReturningType<List<AntibioticListModel>>
{
    public async Task<List<AntibioticListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryGetStringValue("ids", out var idsValue) || string.IsNullOrWhiteSpace(idsValue))
        {
            return [];
        }

        var ids = idsValue
            .Split(',')
            .Select(part => int.TryParse(part.Trim(), out var id) ? id : 0)
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
        {
            return [];
        }

        const string sql = @"SELECT id, antibioticname FROM antibiotic WHERE id = ANY(@ids)";
        var result = await connect.QueryAsync<AntibioticListModel>(sql, new { ids });
        return result.ToList();
    }
}
