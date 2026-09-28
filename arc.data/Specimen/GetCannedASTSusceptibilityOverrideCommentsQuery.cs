using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns canned AST susceptibility override reasons from list 145 (ASTSusOverrideCanned).
/// </summary>
internal class GetCannedASTSusceptibilityOverrideCommentsQuery : IQueryReturningType<List<OptionsConfig>>
{
    private const int AstSusceptibilityOverrideCannedCommentsListId = 145;

    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        const string sql = @"
            SELECT id AS key, value AS text
            FROM listitem
            WHERE listid = @ListId
              AND enabled = true
              AND deleted = false
            ORDER BY displayorder, value";

        var result = await connect.QueryAsync<OptionsConfig>(sql, new { ListId = AstSusceptibilityOverrideCannedCommentsListId });
        return result.ToList();
    }
}
