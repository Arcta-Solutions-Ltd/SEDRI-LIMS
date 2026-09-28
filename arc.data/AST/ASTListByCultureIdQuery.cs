using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.AST
{
    internal class ASTListByCultureIdQuery : IQueryReturningType<List<ASTListByCultureIdModel>>
    {
        /// <summary>
        /// Loads AST rows for the culture record AST list. Parent rows use <c>AST.displayonreport</c>;
        /// special consideration rows use <c>specialastrow.displayonreport</c> (independent of the parent flag).
        /// </summary>
        public async Task<List<ASTListByCultureIdModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var cultureId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "cultureid").First();

            var sql = @"select a.Id, a.DisplayOnReport, concat(coalesce(a.MicComparison,''),a.measurement) as measurement, ant.antibioticname,
                        a.dosage, a.CultureId, li1.Value As Susceptibility, li2.Value As TestMethod, '' As SpecialConsideration, a.testmethodid, 0 As SpecialConsiderationId from AST a 
                        inner join ListItem li1 on li1.id = a.susceptibilityid
                        inner join ListItem li2 on li2.id = a.testmethodid
                        inner join Antibiotic ant on a.antibioticId = ant.id
                        where a.CultureId = @CultureId
                        union all
                        select a.Id, s.displayonreport As DisplayOnReport, concat(coalesce(a.MicComparison,''),a.measurement) as measurement, ant.antibioticname,
                        a.dosage, a.CultureId, li1.Value As Susceptibility, li2.Value As TestMethod, li3.Value As SpecialConsideration, a.testmethodid, s.specialtypeid As SpecialConsiderationId from AST a 
                        inner join SpecialAstRow s on a.id = s.astid
                        inner join ListItem li1 on li1.id = s.susceptibilityid
                        inner join ListItem li2 on li2.id = a.testmethodid
                        inner join ListItem li3 on li3.id = s.specialtypeid
                        inner join Antibiotic ant on a.antibioticId = ant.id
                        where a.CultureId = @CultureId
                        order by id";

            var result = await connect.QueryAsync<ASTListByCultureIdModel>(sql, new { CultureId = int.Parse(cultureId.Value) });
            var resultList = result.ToList();

            foreach (var res in resultList)
            {
                res.Measurement = res.Measurement.Contains("-1") ? "" : res.Measurement;
                res.Dosage = res.TestMethodId == 680 ? "" : res.Dosage;
            }

            return resultList;
        }
    }
}
