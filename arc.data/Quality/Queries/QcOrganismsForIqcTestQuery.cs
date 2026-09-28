using arc.common.Models.Quality;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class QcOrganismsForIqcTestQuery : IQueryReturningType<List<IqcTestProfileQcOrganismsModel>>
    {
        /// <summary>
        /// Given an IQC test ID will return all the QC organisms present in both the test and the associated IQC test profile
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queryFilters">Pass integer ID of IQC test as id</param>
        /// <returns></returns>
        public async Task<List<IqcTestProfileQcOrganismsModel>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql =
            @"SELECT DISTINCT
                qo.id,
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
                case when (ir.qcantibioticid = itpqa.qcantibioticid) then true else false end as presentintest
                FROM 
                iqctests it
                 LEFT JOIN iqcresults ir ON ir.iqctestid = it.id
                 LEFT JOIN iqctestprofiles itp on itp.id = it.iqctestprofileid
                 LEFT JOIN iqctestprofileqcorganisms itpqo ON itpqo.iqctestprofileid = itp.id
                 LEFT JOIN iqctestprofileqcantibiotics itpqa ON itpqa.iqctestprofileqcorganismid = itpqo.id
                 LEFT JOIN qcorganisms qo ON qo.id = itpqo.qcorganismid
                 inner join organism o on qo.organismid = o.id 
	                left outer join genus g on g.Id = o.genusId 
	                left outer join species s on s.Id = o.speciesId 
	                left outer join subspecies ss on ss.id = o.subspeciesId 
	                left outer join serotype se on se.Id = o.serotypeId 
	                left outer join additional a on a.Id = o.additionalId 	
                 WHERE it.id = @id
                 AND ((itp.deleteddate IS NULL AND itpqa.enabled) OR (ir.qcantibioticid = itpqa.qcantibioticid))
                group by itpqo.id, qo.standardsbody, organism, qo.id, g.name, s.name, ss.name, se.name, a.name, ir.qcantibioticid, itpqa.qcantibioticid";

            var result = await connection.QueryAsync<IqcTestProfileQcOrganismsModel>(sql, new { id });

            // Remove any duplicated rows by making rows where presentintest is true take precedent 
            return result.GroupBy(x => x.Id).Select(x => x.OrderByDescending(x => x.PresentInTest == true).First()).ToList();
        }
    }
}
