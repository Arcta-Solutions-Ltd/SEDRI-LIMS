using arc.common.Models.QualityAssurance;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Quality;

internal class IqcTestProfileListQuery : IQueryReturningType<List<IqcTestProfileListModel>>
{
    public async Task<List<IqcTestProfileListModel>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
    {

		queryFilters.TryParseIntegerValue("iqctestprofileid", out var iqcTestProfileId, 0);

		var whereClause = new StringBuilder($"");

        var queryFilterExists = queryFilters.TryGetStringValue("genusname", out var genusName);
        queryFilters.TryGetStringValue("speciesname", out var speciesName);
        queryFilters.TryGetStringValue("standardsbody", out var standardsBody);
        queryFilters.TryGetStringValue("primarystrain", out var primaryStrain);
        queryFilters.TryGetStringValue("otherstrains", out var otherStrains);
        queryFilters.TryGetStringValue("subspeciesname", out var subspecies);
        queryFilters.TryGetStringValue("serotypename", out var serotype);
        queryFilters.TryGetStringValue("additionalname", out var additional);

		if (queryFilterExists)
		{
            whereClause.AppendLine($"""
                                and (
                            	(g.name) ilike '{genusName.Trim().ToSqlStartsWith()}' 
                                or LOWER(s.name) ilike '{speciesName.Trim().ToSqlStartsWith()}' 
                                or LOWER(ss.name) ilike '{subspecies.Trim().ToSqlStartsWith()}' 
                                or LOWER(se.name) ilike '{serotype.Trim().ToSqlStartsWith()}' 
                                or LOWER(a.name) ilike '{additional.Trim().ToSqlStartsWith()}' 
                                or LOWER(qo.standardsbody) ilike '{standardsBody.Trim().ToSqlStartsWith()}' 
                                or LOWER(qo.primarystrain) ilike '{primaryStrain.Trim().ToSqlStartsWith()}' 
                                or LOWER(qo.otherstrains) ilike '{otherStrains.Trim().ToSqlStartsWith()}' 
                            	)
                            """);
        }

        var sql = $"""
			select 
				itpqo.id,
				TRIM(
					CONCAT(
						g.name, 
						case when g.name is not null 
						and s.name is null 
						and a.name is null then ' spp.' else '' end, 
						' ', 
						s.name, 
						' ', 
						ss.name, 
						' ', 
						TRIM(se.name), 
						a.name
					)
				) as organism,
				qo.standardsbody,
				qo.primarystrain, 
				qo.otherstrains, 
				case when itpqo.usebydefault = true then '@GenYesA@' else '@GenNo@' end as usebydefault,
				case when (count(itpqa.enabled)) > 0  then '@GenYesA@' else '@GenNo@' end as enabled 
				from iqctestprofiles itp
				left join iqctestprofileqcorganisms itpqo on itpqo.iqctestprofileid = itp.id
				left outer join iqctestprofileqcantibiotics itpqa on itpqa.iqctestprofileqcorganismid = itpqo.id and itpqa.enabled = true
				left join qcorganisms qo on qo.id = itpqo.qcorganismid
				inner join organism o on qo.organismid = o.id 
				left outer join genus g on g.Id = o.genusId 
				left outer join species s on s.Id = o.speciesId 
				left outer join subspecies ss on ss.id = o.subspeciesId 
				left outer join serotype se on se.Id = o.serotypeId 
				left outer join additional a on a.Id = o.additionalId 	
				where 
				itp.id = @iqcTestProfileId
				and itp.deleteddate is null
				{whereClause}
				group by itpqo.id, qo.standardsbody, organism, itpqo.usebydefault, qo.id, g.name, s.name, ss.name, se.name, a.name
			    order by {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "CONCAT(g.name, s.name, ss.name, se.name, a.name)")} limit 500
			""";

        var result = await connection.QueryAsync<IqcTestProfileListModel>(sql, new { iqcTestProfileId });

        return result.ToList();
    }
}
