using arc.common.Models.AST;

using arc.domain.Configuration.QueryFiltersConfig;

using Dapper;

using Npgsql;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;



namespace arc.data.Coding;



/// <summary>

/// Returns <c>antibiotic.id</c> and group listitem id (<c>antibioticcoding.codingid</c>) for antibiotics in the given groups.

/// </summary>

internal class AntibioticIdsByGroupIdsQuery : IQueryReturningType<List<AntibioticIdGroupPair>>

{

    /// <inheritdoc />

    public async Task<List<AntibioticIdGroupPair>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)

    {

        if (!queryFilters.TryGetStringValue("groupids", out var raw) || string.IsNullOrWhiteSpace(raw))

        {

            return [];

        }



        var ids = raw.Split(',').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse).Distinct().ToArray();

        if (ids.Length == 0)

        {

            return [];

        }



        const string sql = @"

SELECT ac.antibioticid AS Id, ac.codingid AS GroupId

FROM antibioticcoding ac

WHERE ac.codingid = ANY(@Ids)

ORDER BY ac.antibioticid";



        var rows = await connect.QueryAsync<AntibioticIdGroupPair>(sql, new { Ids = ids });

        return rows.ToList();

    }

}

