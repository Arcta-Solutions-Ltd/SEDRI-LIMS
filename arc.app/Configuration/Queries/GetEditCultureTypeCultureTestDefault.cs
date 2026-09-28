//using arc.app.Common;
//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Newtonsoft.Json;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Queries
//{
//    internal class GetEditCultureTypeCultureTestDefault : ISingleConfig
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;
//        private readonly IListRepository _listRepository;

//        internal GetEditCultureTypeCultureTestDefault(IConfigExtractionUtils configExtractionUtils, IListRepository listRepository)
//        {
//            _configExtractionUtils = configExtractionUtils;
//            _listRepository = listRepository;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

//            var idList = id.Split("|");

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));

//            var cultureDefault = view.DefaultCultureTests.Where(c => c.CultureType == int.Parse(idList[1])).First();

//            var cultureName = await _listRepository.GetValueFromIdAsync(cultureDefault.CultureType);

//            var returnValue = new { CultureTypeId = cultureDefault.CultureType, CultureTestId = string.Join(",", cultureDefault.Values), CultureType = cultureName };

//            return JsonConvert.SerializeObject(returnValue);

//        }

//    }
//}
