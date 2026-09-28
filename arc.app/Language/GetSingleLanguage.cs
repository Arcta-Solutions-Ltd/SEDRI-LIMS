using arc.app.Common;
using arc.app.Config.Language;
using arc.app.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Language
{
    public class GetSingleLanguage : ISingleConfig
    {
        private readonly IHandleQuery _queryHandler;
        private readonly ILanguageFactory _languageFactory;

        public GetSingleLanguage(IHandleQuery queryHandler, ILanguageFactory languageFactory)
        {
            _queryHandler = queryHandler;
            _languageFactory = languageFactory;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var result = await _queryHandler.HandleAsync(queryData.Query, queryFilters, queryData);

            // If config does not exist in database get it from the internal structure

            if (string.IsNullOrEmpty(result) || result == "[]")
            {
                if (queryData.Query.Equals("languagelist", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    var languageList = await _languageFactory.GetAsync("669");

                    var filter = queryFilters.Parameters.FirstOrDefault(q => q.Key.Equals("value", System.StringComparison.CurrentCultureIgnoreCase));

                    languageList = filter?.Value != null
                        ? languageList.Where(l => l.Value.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)).ToList()
                        : languageList;

                    result = JsonConvert.SerializeObject(languageList);
                }
            }

            // Send the config back as a result set

            return result;
        }
    }
}
