using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>Result row for <see cref="ConfigNameByFirstIdQuery"/> (required for <c>QueryReturningTypeAsync</c> generic constraint).</summary>
internal class ConfigNameByFirstIdResult
{
    public string Name { get; set; }
}

/// <summary>
/// Returns <c>configs.configname</c> for the first entry in a comma-separated list (direct/culture test config ids on a profile).
/// Matches <see cref="InstrumentProfileConfigIdsMatchQuery"/> semantics: first token may be a numeric <c>configs.id</c> or a <c>configname</c> string.
/// </summary>
internal class ConfigNameByFirstIdQuery : IQueryReturningType<ConfigNameByFirstIdResult>
{
    public async Task<ConfigNameByFirstIdResult> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var idList = queryFilters.GetStringValue("profileids");
        if (string.IsNullOrWhiteSpace(idList))
            return new ConfigNameByFirstIdResult();
        var first = idList.Split(',').Select(s => s.Trim()).FirstOrDefault(s => !string.IsNullOrEmpty(s));
        if (string.IsNullOrEmpty(first))
            return new ConfigNameByFirstIdResult();

        if (int.TryParse(first, out var id))
        {
            var name = await connect.QueryFirstOrDefaultAsync<string>("select configname from configs where id = @Id", new { Id = id });
            return new ConfigNameByFirstIdResult { Name = name };
        }

        const string byNameSql = "select configname from configs where lower(trim(configname)) = lower(trim(@Name)) limit 1";
        var nameByKey = await connect.QueryFirstOrDefaultAsync<string>(byNameSql, new { Name = first });
        return new ConfigNameByFirstIdResult { Name = nameByKey };
    }
}
