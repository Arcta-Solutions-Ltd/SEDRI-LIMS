//using arc.app.Common;
//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Newtonsoft.Json;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.app.Configuration
//{
//    internal class GetEditCultureTypeDefault : ISingleConfig
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;
//        private readonly IListRepository _listRepository;

//        internal GetEditCultureTypeDefault(IConfigExtractionUtils configExtractionUtils, IListRepository listRepository)
//        {
//            _configExtractionUtils = configExtractionUtils;
//            _listRepository = listRepository;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

//            var idList = id.Split("|");

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));

//            var cultureDefault = view.DefaultCultures.Where(c => c.SpecimenType == int.Parse(idList[1])).First();

//            var specimenType = await _listRepository.GetValueFromIdAsync(cultureDefault.SpecimenType);

//            var returnValue = new { SpecimenTypeId = cultureDefault.SpecimenType, CultureTypeId = string.Join(",",cultureDefault.Values), SpecimenType = specimenType };

//            return JsonConvert.SerializeObject(returnValue);

//        }
//    }
//}
