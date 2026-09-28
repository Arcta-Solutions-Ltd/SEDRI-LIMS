using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class IqcTestProfileSingleQuery : IQueryReturningType<IqcTestProfileListModel>
    {
        public async Task<IqcTestProfileListModel> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"
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
				where itpqo.id = @id
				and itp.deleteddate is null
				group by itpqo.id, qo.standardsbody, organism, itpqo.usebydefault, qo.id";

            return await connection.QueryFirstAsync<IqcTestProfileListModel>(sql, new { id });
        }
    }
}
