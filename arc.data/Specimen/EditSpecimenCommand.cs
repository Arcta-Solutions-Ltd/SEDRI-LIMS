using arc.app.Common;
using arc.app.Reports.InclusionSelectors;
using arc.common.Data;
using arc.common.Utils;
using arc.data.Utils;
using arc.domain.Tests;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    internal class EditSpecimenCommand : ICommand
    {
        private readonly IGenerateMoreData _moreDataGenerator;
        private readonly ILogWriter _logWriter;
        private readonly IMoreDataRepository _moreDataRepository;
        private readonly IJsonUtils _jsonUtils;
        private readonly IJsonReplacer _jsonReplacer;
        private readonly IJsonElementRemover _jsonRemover;

        public EditSpecimenCommand(IGenerateMoreData moreDataGenerator, ILogWriter logWriter, IJsonUtils jsonUtils, IMoreDataRepository moreDataRepository, IJsonReplacer jsonReplacer, IJsonElementRemover jsonRemover)
        {
            _moreDataGenerator = moreDataGenerator;
            _logWriter = logWriter;
            _jsonUtils = jsonUtils;
            _moreDataRepository = moreDataRepository;
            _jsonReplacer = jsonReplacer;
            _jsonRemover = jsonRemover;
            _jsonRemover = jsonRemover;
        }

        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var dataToSave = args[0];
            var configName = args[1];
            var newStateId = args[2];

            var reportFilter = JsonConvert.DeserializeObject<ReportFilterModel>(dataToSave);
            dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "ReportFilter");

            var id = _jsonUtils.GetSingleFieldValue(dataToSave, "Id");

            var moreData = _moreDataGenerator.GetMoreDataJsonString(configName, dataToSave);

            if (moreData != "{}")
            {
                _logWriter.LogInfo("More data found and being combined", "EditSpecimenCommand", "Execute");
                moreData = await _moreDataRepository.CombineWithExistingFieldAsync(moreData, configName, id, connect);
                dataToSave = _moreDataGenerator.GetLeftOverData();
            };

            var builder = new SqlBuilder(dataToSave, "");
            _logWriter.LogInfo("Start building the parameterized sql", "EditSpecimenCommand", "Execute");
            var sqlResult = builder.BuildUpdateSqlParameterized(configName, id, moreData);

            if (sqlResult.ExcludedMetadataFields.Count > 0)
            {
                _logWriter.LogInfo($"WARN: Excluded client metadata fields from {configName} SQL: {string.Join(", ", sqlResult.ExcludedMetadataFields)}", "EditSpecimenCommand", "Execute");
            }

            if (!string.IsNullOrWhiteSpace(sqlResult.Sql))
            {
                _logWriter.LogInfo("Execute the sql statement with parameters", "EditSpecimenCommand", "Execute");
                await connect.ExecuteAsync(sqlResult.Sql, sqlResult.Parameters);
            }

            var sql = @"update specimen set StateId = @StateId, lastmodifieddate = now() where id = @Id";
            await connect.ExecuteAsync(sql, new { StateId = int.Parse(newStateId), Id = int.Parse(id) });

            sql = @"insert into SpecimenStateHistory(StateId, SpecimenId, LastModifiedDate) values(@StateId, @SpecimenId, now())";
            await connect.ExecuteAsync(sql, new { StateId = int.Parse(newStateId), SpecimenId = int.Parse(id) });

            if (reportFilter != null && reportFilter.ReportFilter != null)
            {
                foreach (var item in reportFilter.ReportFilter.Cultures)
                {
                    sql = @"update culture set DisplayOnReport = @Display where Id = @Id";
                    await connect.ExecuteAsync(sql, new { Id = int.Parse(item.Id), Display = item.PrintOnReport });
                }

                foreach (var test in reportFilter.ReportFilter.DirectTestList)
                {
                    sql = "select * from tests where Id = @Id";
                    var testRecord = await connect.QueryFirstAsync<Test>(sql, new { Id = int.Parse(test.Id) });
                    var contents = _jsonReplacer.ChangeValueInJsonString(testRecord.TestResults, "printonreport", test.PrintOnReport);

                    sql = @"update tests set TestResults = cast(@Contents as json) where Id = @Id";
                    await connect.ExecuteAsync(sql, new { Id = int.Parse(test.Id), contents });
                }

                foreach(var comment in reportFilter.ReportFilter.Comments)
                {
                    sql = @"update specimencomment set DisplayOnReport = @Display where Id = @Id";
                    await connect.ExecuteAsync(sql, new { Id = int.Parse(comment.Id), Display = comment.PrintOnReport });
                }
            }
            return 0;
        }
    }
}
