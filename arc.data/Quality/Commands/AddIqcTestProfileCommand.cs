using arc.app.Common;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality.Commands
{
    internal class AddIqcTestProfileCommand : ICommandWithTypeReturningInteger<IqcTestProfile>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, IqcTestProfile iqcTestProfile, ILogWriter logWriter)
        {
            var sql = $@"INSERT INTO iqctestprofiles(
                        name, testmethodlistitemid, lastmodifieddate)
	                    VALUES (@Name, @TestMethodListItemId, now()) returning id;";

            var iqcTestProfileId = await connection.QuerySingleAsync<int>(sql, iqcTestProfile);

            foreach (var iqcTestProfileQcOrganism in iqcTestProfile.IqcTestProfileQcOrganisms)
            {
                iqcTestProfileQcOrganism.IqcTestProfileId = iqcTestProfileId;
                sql = $@"INSERT INTO iqctestprofileqcorganisms(
                        iqctestprofileid, qcorganismid,  usebydefault, lastmodifieddate)
	                    VALUES (@IqcTestProfileId, @QcOrganismId, @UseByDefault, now()) returning id;";

                var iqcTestProfileQcOrganismsId = await connection.QuerySingleAsync<int>(sql, iqcTestProfileQcOrganism);

                foreach (var iqcTestProfileQcAntibiotic in iqcTestProfileQcOrganism.IqcTestProfileQcAntibiotics)
                {
                    iqcTestProfileQcAntibiotic.IqcTestProfileQcOrganismId = iqcTestProfileQcOrganismsId;
                    sql = $@"INSERT INTO iqctestprofileqcantibiotics(
                        iqctestprofileqcorganismid, qcantibioticid, enabled, lastmodifieddate)
	                    VALUES (@IqcTestProfileQcOrganismId, @QcAntibioticId, @Enabled, now()) returning id;";
                    await connection.QueryAsync<int>(sql, iqcTestProfileQcAntibiotic);
                }
            }

            return iqcTestProfileId;
        }
    }
}

