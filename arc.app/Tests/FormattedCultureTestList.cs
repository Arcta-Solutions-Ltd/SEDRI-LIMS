using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    public class FormattedCultureTestList : IFormattedCultureTestList
    {
        private readonly IQueryAdapter _queryAdapter;
        private readonly IHandleQuery _queryHandler;
        private readonly IConvertJsonStructureToKeyValuePair _pairConverter;
        private readonly IConfigItemHandler _configItemHandler;

        public FormattedCultureTestList(IQueryAdapter queryAdapter, IHandleQuery queryHandler, IConvertJsonStructureToKeyValuePair pairConverter, IConfigItemHandler configItemHandler)
        {
            _queryAdapter = queryAdapter;
            _queryHandler = queryHandler;
            _pairConverter = pairConverter;
            _configItemHandler = configItemHandler;
        }

        public async Task<List<KeyValueModel>> GetListAsync(List<CultureTest> cultureTestList, TokenInfoModel token)
        {
            var savedTests = "";
            var returnList = new List<KeyValueModel>();
            foreach (var test in cultureTestList)
            {
                if (test.Status == "Complete" && !savedTests.Contains("," + test.TestName.ToLower() + ","))
                {
                    var contents = _configItemHandler.GetSingleItemAsync(test.TestName).Result;
                    var testName = JsonConvert.DeserializeObject<InitialQueryModel>(contents).InitialQuery;

                    var queryData = await _queryAdapter.GetQueryAsync(testName);
                    var queryFilters = new QueryFilterConfig
                    {
                        Name = testName,
                        Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = test.Id.ToString() } }
                    };

                    var result = await _queryHandler.HandleAsync(testName, queryFilters, queryData, token);
                    var testValues = _pairConverter.Convert(result, true).ToList();

                    returnList.AddRange(testValues);

                    savedTests += "," + test.TestName.ToLower() + ",";
                }
            }

            return returnList;
        }

        public List<KeyValueModel> TranslateHeadings(List<KeyValueModel> listToConvert, Dictionary<string, string> conversions)
        {
            foreach (var item in listToConvert)
            {
                if (item.Key != null)
                {
                    if (conversions.ContainsKey(item.Key.ToLower()))
                    {
                        item.Key = conversions[item.Key.ToLower()];
                    }
                }
            }

            return listToConvert.Where(i => i.Key != "printonreport").ToList();
        }
    }
}
