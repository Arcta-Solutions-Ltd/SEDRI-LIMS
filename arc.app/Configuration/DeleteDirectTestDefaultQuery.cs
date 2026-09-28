//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using arc.app.Common;
//using Newtonsoft.Json;

//namespace arc.app.Configuration
//{
//    internal class DeleteDirectTestDefaultQuery : ISingleConfig
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;
//        private readonly IServiceProvider _serviceProvider;

//        internal DeleteDirectTestDefaultQuery(IServiceProvider serviceProvider, IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

//            var idList = id.Split("|");

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));

//            var testDefault = view.DefaultTests.Where(c => c.SpecimenType == int.Parse(idList[1])).First();

//            var listRepository = _serviceProvider.GetService<IListRepository>();
//            var specimenType = await listRepository.GetValueFromIdAsync(testDefault.SpecimenType);

//            var returnValue = new { Id = testDefault.SpecimenType, SpecimenTypeId = specimenType };

//            return JsonConvert.SerializeObject(returnValue);

//        }
//    }
//}
