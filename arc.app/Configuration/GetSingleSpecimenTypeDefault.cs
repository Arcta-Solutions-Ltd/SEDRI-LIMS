//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Newtonsoft.Json;
//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using arc.app.Common;
//using System.Linq;

//namespace arc.app.Configuration
//{
//    internal class GetSingleSpecimenTypeDefault : ISingleConfig
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        internal GetSingleSpecimenTypeDefault(IServiceProvider serviceProvider, IConfigExtractionUtils configExtractionUtils)
//        {
//            _serviceProvider = serviceProvider;
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var id = queryFilters.Parameters[0].Value;

//            var idArray = id.Split("|");

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idArray[0]));

//            var listRepository = _serviceProvider.GetService<IListRepository>();

//            var foundItem = view.DefaultCultures.Where(c => c.SpecimenType == int.Parse(idArray[1])).First();

//            var specimenType = await listRepository.GetValueFromIdAsync(foundItem.SpecimenType);

//            var cultureTypeList = "";
//            foreach (var cultureType in foundItem.Values)
//            {
//                {
//                    var cultureDesc = await listRepository.GetValueFromIdAsync(int.Parse(cultureType));

//                    cultureTypeList += cultureTypeList == "" ? cultureDesc : ", " + cultureDesc;
//                }
//            }

//            var returnItem = new { SpecimenType = specimenType, CultureTypeList = cultureTypeList, Id = id };
//            return JsonConvert.SerializeObject(returnItem);
//        }
//    }
//}
