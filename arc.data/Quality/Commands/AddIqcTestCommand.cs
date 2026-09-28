using arc.app.Common;
using arc.common.Models.Quality;
using arc.data.Quality.Queries;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class AddIqcTestCommand : ICommandWithTypeReturningInteger<AddIqcTestModel>
    {
        private NpgsqlConnection _connection;
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, AddIqcTestModel addIqcTestModel, ILogWriter logWriter)
        {
            _connection = connection;

            var iqcTestProfile = await GetIqcTestProfileById(addIqcTestModel.TestProfileId);

            return await AddIqcTestFromIqcTestProfile(addIqcTestModel, iqcTestProfile);
        }

        private async Task<IqcTestProfile> GetIqcTestProfileById(int iqcTestProfileId)
        {
            var queryFilterConfig = new QueryFilterConfig();
            queryFilterConfig.AddInteger("iqctestprofileid", iqcTestProfileId);
            return await new GetIqcTestProfileByIdQuery().ExecuteAsync(_connection, queryFilterConfig);
        }

        private async Task<int> AddIqcTestFromIqcTestProfile(AddIqcTestModel addIqcTestModel, IqcTestProfile iqcTestProfile)
        {
            var pendingStateIdSql = @"SELECT li.id from listitem li
                                    LEFT JOIN list l on l.id = li.listid
                                    WHERE l.name = 'IqcTestState'
                                    AND li.value = 'Pending'";
            var sql = @$"insert into iqctests (stateid, iqctestprofileid, createddate, lastmodifieddate)
                        values (({pendingStateIdSql}), @Id, now(), now()) returning id";

            var iqcTestId = await _connection.QueryFirstAsync<int>(sql, new { iqcTestProfile.Id });

            foreach (var iqcTestProfileQcOrganism in iqcTestProfile.IqcTestProfileQcOrganisms)
            {
                foreach (var iqcTestProfileQcAntibiotic in iqcTestProfileQcOrganism.IqcTestProfileQcAntibiotics)
                {
                    if (addIqcTestModel.IqcTestProfileQcOrganismIds.Contains(iqcTestProfileQcOrganism.Id) && iqcTestProfileQcAntibiotic.Enabled)
                    {
                        sql = @"insert into iqcresults (iqctestid, qcantibioticid, lastmodifieddate)
                            values (@iqcTestId, @acAntibioticId, now())";

                        await _connection.QueryAsync(sql, new { iqcTestId, acAntibioticId = iqcTestProfileQcAntibiotic.QcAntibioticId });
                    }
                }
            }

            sql = $@"UPDATE iqctests
                        SET accessionnumber = @accessionNumber
                        WHERE id = @iqcTestId;";

            await _connection.QueryAsync(sql, new { accessionNumber = $"IQC{iqcTestId.ToString().PadLeft(7, '0')}", iqcTestId });

            return iqcTestId;
        }
    }
}

