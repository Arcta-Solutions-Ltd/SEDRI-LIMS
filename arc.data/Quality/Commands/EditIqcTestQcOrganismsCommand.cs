using arc.app.Common;
using arc.common.Models.QualityAssurance;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class EditIqcTestQcOrganismsCommand : ICommandWithTypeReturningInteger<IqcTestModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, IqcTestModel command, ILogWriter logWriter)
        {
            foreach (var item in command.Crafted[0].Contents)
            {
                var qcOrganismId = int.Parse(item.Key);
                var iqcTestId = command.Id;

                if (item.Allowed == "Yes")
                {
                    var qcAntibioticIdsSql = @"SELECT qa.id from qcantibiotics qa 
                    LEFT JOIN iqctestprofileqcantibiotics itpqa ON itpqa.qcantibioticid = qa.id
                    LEFT JOIN iqctestprofileqcorganisms itpqo ON itpqo.id = itpqa.iqctestprofileqcorganismid
                    LEFT JOIN iqctestprofiles itp on itp.id = itpqo.iqctestprofileid
                    LEFT JOIN iqctests it on it.iqctestprofileid = itp.id
                    WHERE it.id = @iqcTestId AND qa.qcorganismid = @qcOrganismId AND itpqa.enabled = true AND itp.deleteddate IS NULL";
                    var qcAntibioticIdsResult = await connection.QueryAsync<int>(qcAntibioticIdsSql, new { qcOrganismId, iqcTestId });
                    int[] qcAntibioticIds = qcAntibioticIdsResult.ToArray();

                    var existingResultsSql = "select qcantibioticid from iqcresults where iqctestid = @iqcTestId and qcantibioticid = ANY (@qcAntibioticIds)";
                    var existingMatchingQcAntibioticIds = await connection.QueryAsync<int>(existingResultsSql, new { iqcTestId, qcAntibioticIds });

                    foreach (var qcAntibioticId in qcAntibioticIdsResult)
                    {
                        if (!existingMatchingQcAntibioticIds.Contains(qcAntibioticId))
                        {
                            var insertSql = "insert into iqcresults(iqctestid, qcantibioticid, lastmodifieddate) values(@IqcTestId, @qcAntibioticId, now())";
                            await connection.ExecuteAsync(insertSql, new { @IqcTestId = command.Id, qcAntibioticId });
                        }
                        // Have chosen not to delete results where the IQC Test Profile has been amended such that antibiotics are no longer associated with the qc organism
                    }
                }
                else
                {
                    var qcAntibioticIdsToDelete = await connection.QueryAsync<int>("select id from qcantibiotics where qcorganismid = @qcOrganismId", new { qcOrganismId });
                    await connection.ExecuteAsync("delete from iqcresults where iqctestid = @iqcTestId and qcantibioticid = ANY ( @qcAntibioticIds )", new { iqcTestId, qcAntibioticIds = qcAntibioticIdsToDelete.ToArray() });
                }
            }

            return command.Id;
        }
    }
}
