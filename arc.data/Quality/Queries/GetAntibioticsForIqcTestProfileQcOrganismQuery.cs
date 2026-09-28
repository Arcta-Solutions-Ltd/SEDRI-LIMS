using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    public class GetAntibioticsForIqcTestProfileQcOrganismQuery : IQueryReturningType<List<OrganismAntibioticModel>>
    {
        public async Task<List<OrganismAntibioticModel>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select 
                itpqa.id, 
                a.antibioticname, 
                itpqa.enabled
                from iqctestprofileqcantibiotics itpqa
                inner join qcantibiotics qa on qa.id = itpqa.qcantibioticid 
                inner join antibiotic a on a.id = qa.antibioticid
                where iqctestprofileqcorganismid = @id";

            var result = await connection.QueryAsync<OrganismAntibioticModel>(sql, new { id });

            return result.ToList();
        }
    }
}
