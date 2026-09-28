using arc.app.Common;
using arc.common.Models.QualityAssurance;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class EditIqcTestProfileCommand : ICommandWithTypeReturningInteger<QualityCraftedModel>
    {
        private NpgsqlConnection _connection;

        public async Task<int> ExecuteAsync(NpgsqlConnection connection, QualityCraftedModel command, ILogWriter logWriter)
        {
            var qcOrganismId = command.Id;
            var useByDefault = command.Default == "Yes";
            _connection = connection;

            await SetIqcTestProfileQcOrganismUseByDefault(qcOrganismId, useByDefault);

            foreach (var item in command.Crafted[0].Contents)
            {
                await SetIqcTestProfileQcAntibioticEnabled(int.Parse(item.Key), item.Allowed == "Yes");
            }
            return qcOrganismId;
        }

        //TODO: Move this to seperate commands and crafted model into the handler

        private async Task SetIqcTestProfileQcOrganismUseByDefault(int qcOrganismId, bool useByDefault)
        {
            var sql = @"update iqctestprofileqcorganisms 
                        set usebydefault = @usebydefault, lastmodifieddate = now()
                        where id = @qcOrganismId";
            await _connection.QueryAsync(sql, new { qcOrganismId, useByDefault });
        }

        private async Task SetIqcTestProfileQcAntibioticEnabled(int qcAntibioticId, bool enabled)
        {
            var sql = @"update iqctestprofileqcantibiotics 
                        set enabled = @enabled, lastmodifieddate = now()
                        where id = @qcAntibioticId";
            await _connection.QueryAsync(sql, new { qcAntibioticId, enabled });
        }
    }
}
