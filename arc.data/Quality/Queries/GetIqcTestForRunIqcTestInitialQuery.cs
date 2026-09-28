using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class GetIqcTestForRunIqcTestInitialQuery : IQueryReturningType<EditIqcTestModel>
    {
        public async Task<EditIqcTestModel> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"SELECT 
              it.id, 
              li.value as ""TestMethod"",
              TRIM(
                CONCAT(
                  g.name, 
                  case when g.name is not null 
                  and s.name is null 
                  and ad.name is null then ' spp.' else '' end, 
                  ' ', 
                  s.name, 
                  ' ', 
                  ss.name, 
                  ' ', 
                  TRIM(se.name), 
                  ad.name
                )
              ) AS ""QcOrganismName"", 
              qco.standardsbody AS ""StandardsBody"",
			  qco.primarystrain AS ""PrimaryStrain"",
              qco.id AS ""QcOrganismId"",
              ir.id AS ""IqcResultId"",
			  a.antibioticname AS ""AntibioticName"",
			  ir.value AS ""ResultValue"",
			  case when li.value = 'MIC' then qa.micrangelower else qa.inhibitionzonediameterrangelower end as ""ExpectedLowerValue"",
			  case when li.value = 'MIC' then qa.micrangeupper else qa.inhibitionzonediameterrangeupper end as ""ExpectedUpperValue""
              FROM 
			  iqctests it 
			  LEFT JOIN iqcresults ir ON it.id = ir.iqctestid
			  LEFT JOIN qcantibiotics qa ON qa.id = ir.qcantibioticid 
			  LEFT JOIN antibiotic a ON a.id = qa.antibioticid
              LEFT JOIN qcorganisms qco ON qco.id =  qa.qcorganismid
              INNER JOIN organism og ON qco.organismid = og.id 
              LEFT OUTER JOIN genus g ON g.Id = og.genusId 
              LEFT OUTER JOIN species s ON s.Id = og.speciesId 
              LEFT OUTER JOIN subspecies ss ON ss.id = og.subspeciesId 
              LEFT OUTER JOIN serotype se ON se.Id = og.serotypeId 
              LEFT OUTER JOIN additional ad ON ad.Id = og.additionalId
              INNER JOIN iqctestprofiles itp on itp.id = it.iqctestprofileid
              INNER JOIN listitem li ON li.id = itp.testmethodlistitemid
              WHERE it.id = @Id;
			  ";

            var results = await connection.QueryAsync<dynamic>(sql, new { Id = id });

            var response = new EditIqcTestModel() { Id = id };
            foreach (var result in results)
            {
                var existing = response.QcOrganismWithIqcResults.Where(x => x.QcOrganismName == result.QcOrganismName).FirstOrDefault();
                if (existing == null)
                {
                    var resultValue = result.ResultValue;
                    var qcOrganismWithIqcResults = new QcOrganismWithIqcResultsModel()
                    {
                        QcOrganismId = result.QcOrganismId,
                        QcOrganismName = result.QcOrganismName,
                        StandardsBody = result.StandardsBody,
                        PrimaryStrain = result.PrimaryStrain
                    };
                    qcOrganismWithIqcResults.IqcResults.Add(
                    new IqcResultModel()
                    {
                        AntibioticName = result.AntibioticName,
                        IqcResultId = result.IqcResultId,
                        ResultValue = result.ResultValue,
                        TestMethod = result.TestMethod,
                        ExpectedLowerValue = result.ExpectedLowerValue,
                        ExpectedUpperValue = result.ExpectedUpperValue
                    });

                    response.QcOrganismWithIqcResults.Add(qcOrganismWithIqcResults);
                }
                else
                {
                    existing.IqcResults.Add(new IqcResultModel()
                    {
                        AntibioticName = result.AntibioticName,
                        IqcResultId = result.IqcResultId,
                        ResultValue = result.ResultValue,
                        TestMethod = result.TestMethod,
                        ExpectedLowerValue = result.ExpectedLowerValue,
                        ExpectedUpperValue = result.ExpectedUpperValue
                    });
                }

            }
            return response;
        }
    }
}
