using arc.app.Config.Mapper;
using arc.app.Laboratory;
using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Handles query execution and processing.
    /// </summary>
    public class HandleQuery : IHandleQuery
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISpecialFactory _specialFactory;
        private readonly IMapperAdapter _mapperAdapter;
        private readonly IProcessMapping _processMapping;
        private readonly ILogWriter _logWriter;
        private readonly IStandardFilters _standardFilters;
        private readonly IPostParameterMappingHandlerFactory _postMappingHandlerFactory;
        private readonly ISpecimenListTatEnricher _specimenListTatEnricher;
        private readonly IAgeDisplayQueryResultEnricher _ageDisplayQueryResultEnricher;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleQuery"/> class.
        /// </summary>
        public HandleQuery(IGenericRepository genericRepository, ISpecialFactory specialFactory, IMapperAdapter mapperAdapter, IProcessMapping processMapping, ILogWriter logWriter, IStandardFilters standardFilters, IPostParameterMappingHandlerFactory postMappingHandlerFactory, ISpecimenListTatEnricher specimenListTatEnricher, IAgeDisplayQueryResultEnricher ageDisplayQueryResultEnricher)
        {
            _genericRepository = genericRepository;
            _specialFactory = specialFactory;
            _mapperAdapter = mapperAdapter;
            _processMapping = processMapping;
            _logWriter = logWriter;
            _standardFilters = standardFilters;
            _postMappingHandlerFactory = postMappingHandlerFactory;
            _specimenListTatEnricher = specimenListTatEnricher;
            _ageDisplayQueryResultEnricher = ageDisplayQueryResultEnricher;
        }

        /// <summary>
        /// Asynchronously handles a query based on the provided parameters, filters, and configuration.
        /// </summary>
        /// <param name="parameters">The query parameters as a JSON string.</param>
        /// <param name="queryFilters">The query filter condtions for the query.</param>
        /// <param name="queryData">The query configuration data.</param>
        /// <param name="token">The token information model.</param>
        /// <param name="excludeTokenFilters">Whether to exclude token filters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the query result as a string.</returns>
        public async Task<string> HandleAsync(string parameters, QueryFilterConfig queryFilters, QueryConfig queryData, TokenInfoModel token, bool excludeTokenFilters = false)
        {
            _logWriter.LogInfo($"Carrying out query : {queryData.Query}", "HandleQuery", "Handle");

            if (!string.IsNullOrEmpty(queryData.ParameterMapping))
            {
                _logWriter.LogInfo($"Map the query parameters using mapper {queryData.ParameterMapping}", "HandleQuery", "Handle");
                var mapperDef = await _mapperAdapter.GetMapperAsync(queryData.ParameterMapping);
                var message = mapperDef.Map(parameters);
                queryFilters = JsonConvert.DeserializeObject<QueryFilterConfig>(message);

                var postMappingHandler = _postMappingHandlerFactory.GetHandler(queryFilters?.Name);
                if (postMappingHandler != null)
                    await postMappingHandler.HandleAsync(queryFilters.Name, queryFilters);
            }

            _genericRepository.AddConfiguration(queryData.TableName);
            _logWriter.LogInfo("Update token", "HandleQuery", "Handle");
            token = await _standardFilters.UpdateTokenAsync(token);

            if (! excludeTokenFilters)
            {
                queryFilters.AddTokenFilter(queryData.TableName, token);
            }

            _logWriter.LogInfo("Update filter", "HandleQuery", "Handle");
            queryFilters = await _standardFilters.UpdateFilterAsync(queryFilters);
            _genericRepository.AddToken(token);

            var result = "";
            switch (queryData.Type.ToLower())
            {
                case "select":
                    _logWriter.LogInfo("Select dataset using the generic handler", "HandleQuery", "Handle");
                    result = await _genericRepository.GetListAsync(queryData, queryFilters);
                    if (string.Equals(queryData.Query, "SpecimenList", StringComparison.OrdinalIgnoreCase))
                    {
                        result = await _specimenListTatEnricher.EnrichAsync(result);
                    }
                    else if (string.Equals(queryData.Query, "SpecimenArchiveList", StringComparison.OrdinalIgnoreCase))
                    {
                        result = await _specimenListTatEnricher.EnrichAsync(result, archiveList: true);
                    }
                    break;
                case "single":
                    _logWriter.LogInfo("Select a single record using the generic handler", "HandleQuery", "Handle");
                    result = await _genericRepository.GetSingleAsync(queryData, queryFilters);
                    if (!string.IsNullOrEmpty(result) &&
                        string.Equals(queryData.Query, "SingleSpecimenForSpecimenList", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var wrapped = "[" + result + "]";
                            var enriched = await _specimenListTatEnricher.EnrichAsync(wrapped);
                            if (!string.IsNullOrEmpty(enriched))
                            {
                                var arr = JArray.Parse(enriched);
                                if (arr.Count > 0)
                                    result = arr[0].ToString(Formatting.None);
                            }
                        }
                        catch (Exception)
                        {
                            // Use unenriched single result
                        }
                    }
                    break;
                case "count":
                    _logWriter.LogInfo("Count the records in a dataset using the generic handler", "HandleQuery", "Handle");
                    result = _genericRepository.GetCountAsync(queryData, queryFilters, excludeTokenFilters).ToString();
                    break;
                case "config":
                case "special":
                    _logWriter.LogInfo($"Selecting data using a special query handler : {queryData.Query}", "HandleQuery", "Handle");
                    //queryFilters.AddTokenFilter(token);
                    result = await _specialFactory.RunQueryAsync(queryFilters.Name, queryFilters, token);
                    break;
            }

            _logWriter.LogInfo("Query completed successfully", "HandleQuery", "Handle");

            if (!string.IsNullOrEmpty(queryData.ResultMapping))
            {
                _logWriter.LogInfo($"Get the query result mapper {queryData.ResultMapping}", "HandleQuery", "Handle");
                var mapperDef = await _mapperAdapter.GetMapperAsync(queryData.ResultMapping);
                if (result != "")
                {
                    var rawQueryResult = result;
                    _logWriter.LogInfo("Process the query results through the mapper", "HandleQuery", "Handle");
                    result = await _processMapping.ProcessAsync(mapperDef, result);

                    if (string.Equals(queryData.Query, "PatientForPatientView", StringComparison.OrdinalIgnoreCase))
                    {
                        result = _ageDisplayQueryResultEnricher.EnrichPatientViewAge(result, rawQueryResult);
                    }
                    else if (string.Equals(queryData.Query, "SpecimenForSpecimenView", StringComparison.OrdinalIgnoreCase))
                    {
                        result = _ageDisplayQueryResultEnricher.EnrichSpecimenViewAge(result, rawQueryResult);
                    }
                }
            }

            var returnResult = result.Replace("\"null\"", "null");

            return returnResult;
        }
    }

}
