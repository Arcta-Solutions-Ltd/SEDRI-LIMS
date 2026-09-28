//using arc.app.Common;
//using arc.app.Exports;
//using arc.common.Models;
//using arc.common.Models.Export;
//using arc.common.Utils;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.data.Exports
//{
//    public class ExportRunRepository : IExportRunRepository
//    {
//        private readonly ISqlCommand _sqlCommand;
//        private readonly ISqlQuery _sqlQuery;
//        private readonly IExportProfileFieldRepository _exportProfileFieldRepository;
//        private readonly ILogWriter _logWriter;

//        public ExportRunRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, IExportProfileFieldRepository exportProfileFieldRepository, ILogWriter logWriter)
//        {
//            _sqlCommand = sqlCommand;
//            _sqlQuery = sqlQuery;
//            _exportProfileFieldRepository = exportProfileFieldRepository;
//            _logWriter = logWriter;
//        }
//        public async Task<ExportRunResponseModel> RunExportAsync(QueryFilterConfig queryFilterConfig, TokenInfoModel token)
//        {
//            var queryFilterForFields = new QueryFilterConfig
//            {
//                Parameters = new List<QueryValuesConfig>
//                {
//                    new QueryValuesConfig
//                    {
//                        Key ="id",
//                        Value = GetKeyValueFromQueryFilter(queryFilterConfig,"ExportProfileId")
//                    }
//                }
//            };
//            var fields = await _exportProfileFieldRepository.GetByProfileIdAsync(queryFilterForFields);
//            return await _sqlQuery.QueryReturningTypeAsync(new ExportRunQuery(fields, token), "Run Export Run Query", queryFilterConfig);
//        }
//        private string GetKeyValueFromQueryFilter(QueryFilterConfig queryFilters, string key)
//        {

//            var id = queryFilters.Parameters.FirstOrDefault(a => a.Key.Is(key));

//            return id?.Value;
//        }
//    }
//}
