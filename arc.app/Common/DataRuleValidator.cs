using arc.app.Config.Queries;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Validates event data against configurable data rules before the event is executed.
    /// Runs queries defined in event DataRules (e.g. NoRecord, FindRecord) and returns a validation message if the rule fails.
    /// </summary>
    public class DataRuleValidator : IDataRuleValidator
    {
        private readonly IQueryAdapter _queryAdapter;
        private readonly IHandleQuery _queryHandler;
        private readonly ILogWriter _logWriter;

        public DataRuleValidator(IQueryAdapter queryAdapter, IHandleQuery queryHandler, ILogWriter logWriter)
        {
            _queryAdapter = queryAdapter;
            _queryHandler = queryHandler;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Validates the event message against all DataRules defined in the event configuration.
        /// Each rule runs a query and checks the result (e.g. NoRecord fails if count &gt; 0).
        /// </summary>
        /// <param name="message">The event message JSON containing field values for the rule queries.</param>
        /// <param name="eventData">The event configuration containing the DataRules to validate.</param>
        /// <param name="token">Token information for the current user.</param>
        /// <returns>A validation error message if any rule fails, or an empty string if validation passes.</returns>
        public async Task<string> ValidateMessageAsync(string message, EventConfig eventData, TokenInfoModel token)
        {
            if (eventData.DataRules == null) { return ""; }

            foreach (var rule in eventData.DataRules)
            {
                var jsonLoader = new JsonWholeStructureFieldsCollector();
                var jsonFields = jsonLoader.GetStructure(message);

                var queryFilters = new QueryFilterConfig
                {
                    Name = rule.Query,
                    Parameters = jsonFields.Select((f) => new QueryValuesConfig { Key = f.Label, Value = f.Contents }).ToList()
                };

                if (CheckRequiredFieldsAreThere(queryFilters.Parameters, rule.RequiredFields))
                {
                    var queryData = await _queryAdapter.GetQueryAsync(queryFilters.Name);
                    if (queryData == null)
                    {
                        _logWriter.LogInfo($"Data rule query '{queryFilters.Name}' is not configured.", "DataRuleValidator", "ValidateMessageAsync");
                        continue;
                    }
                    var result = await _queryHandler.HandleAsync(message, queryFilters, queryData, token, true);

                    switch (rule.Type.ToLower())
                    {
                        case "norecord":
                            if (int.Parse(result) > 0)
                            {
                                return rule.Message;
                            }
                            break;
                        case "findrecord":
                            if (int.Parse(result) < 1)
                            {
                                return rule.Message;
                            }
                            break;
                        case "onerecord":
                            if (int.Parse(result) == 1)
                            {
                                return rule.Message;
                            }
                            break;
                    }
                }
            }
            return "";
        }

            private bool CheckRequiredFieldsAreThere(List<QueryValuesConfig> queryValues, string requiredFields)
            {
                if (!string.IsNullOrEmpty(requiredFields))
                {
                    var found = false;
                    var listOfRequiredFields = requiredFields.Split(",");
                    foreach (var field in listOfRequiredFields)
                    {
                        found = found || queryValues.Any(v => v.Key.ToLower() == field.ToLower() && !string.IsNullOrWhiteSpace(v.Value));
                    }
                    return found;
                }
                return true;
            }

        }
    }
