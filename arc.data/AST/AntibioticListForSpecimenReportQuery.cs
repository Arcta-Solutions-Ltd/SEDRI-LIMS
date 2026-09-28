using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.AST
{
    internal class AntibioticListForSpecimenReportQuery : IQueryReturningType<List<AntibioticListForReportModel>>
    {
        public async Task<List<AntibioticListForReportModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var cultureId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "cultureid").First();

            var sql = @"select a.Id, ant.antibioticname as antibiotic,li.Value as susceptibility, a.DisplayOnReport, concat(coalesce(a.MicComparison,''),a.measurement) as measurement,
                        a.dosage, a.testmethodid, a.guidelinesid, a.expertruleid, ant.code as antibioticcode, '' As SpecialConsideration, '' As SpecialDisplayOnReport, 0 As SpecialConsiderationId from AST a 
                        inner join Antibiotic ant on a.AntibioticId = ant.Id
                        left outer join ListItem li on li.Id = a.SusceptibilityId
                        where CultureId = @CultureId
                        union all
                        select a.Id, ant.antibioticname as antibiotic,li1.Value as susceptibility, a.DisplayOnReport, concat(coalesce(a.MicComparison,''),a.measurement) as measurement,
                        a.dosage, a.testmethodid, a.guidelinesid, a.expertruleid, ant.code as antibioticcode, li3.Value As SpecialConsideration, s.displayonreport As SpecialDisplayOnreport, s.specialtypeid As SpecialConsiderationId from AST a 
                        inner join SpecialAstRow s on a.id = s.astid
                        inner join ListItem li1 on li1.id = s.susceptibilityid
                        inner join ListItem li3 on li3.id = s.specialtypeid
                        inner join Antibiotic ant on a.antibioticId = ant.id
                        where a.CultureId = @CultureId
                        order by testmethodid, id";

            var result = await connect.QueryAsync<AntibioticListForReportModel>(sql, new { CultureId = int.Parse(cultureId.Value) });
            return result.ToList();
        }
    }
}
