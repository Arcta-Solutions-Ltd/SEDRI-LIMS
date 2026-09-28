//using arc.app.Common;
//using arc.app.Import;
//using arc.common.Models.Export;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System.Threading.Tasks;

//namespace arc.data.Import
//{
//    public class ImportRepository : IImportRepository
//    {
//        private readonly ISqlCommand _sqlCommand;
//        private readonly ISqlQuery _sqlQuery;
//        private readonly ILogWriter _logWriter;

//        public ImportRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
//        {
//            _sqlCommand = sqlCommand;
//            _sqlQuery = sqlQuery;
//            _logWriter = logWriter;
//        }

//        public async Task<ExportProfileModel> EditImportProfileQueryAsync(QueryFilterConfig queryFilters)
//        {
//            _logWriter.LogInfo("Run get import profile query", "ImportProfileRepository", "EditImportProfileQueryAsync");
//            return await _sqlQuery.QueryReturningTypeAsync(new EditImportProfileQuery(), "Edit Import Profile Query", queryFilters);
//        }

//        public async Task<int> AddImportProfileAsync(ExportProfileModel dataToSave)
//        {
//            _logWriter.LogInfo("Run add import profile command", "ImportProfileRepository", "AddImportProfileAsync");
//            return await _sqlCommand.CommandWithTypeQueryAsync(new AddImportProfileCommand(), "Insert Import Profile", dataToSave);
//        }
//    }
//}
