//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System;
//using Microsoft.Extensions.DependencyInjection;
//using System.Linq;
//using System.Threading.Tasks;
//using arc.app.Common;
//using Newtonsoft.Json;

//namespace arc.app.Configuration.Queries
//{
//    internal class DeleteCultureTypeCultureTestDefaultQuery : ISingleConfig
//    {
//        private readonly IConfigExtractionUtils _configExtractionUtils;
//        private readonly IServiceProvider _serviceProvider;

//        internal DeleteCultureTypeCultureTestDefaultQuery(IServiceProvider serviceProvider, IConfigExtractionUtils configExtractionUtils)
//        {
//            _configExtractionUtils = configExtractionUtils;
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

//            var idList = id.Split("|");

//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));

//            var testDefault = view.DefaultCultureTests.Where(c => c.CultureType == int.Parse(idList[1])).First();

//            var listRepository = _serviceProvider.GetService<IListRepository>();
//            var cultureType = await listRepository.GetValueFromIdAsync(testDefault.CultureType);

//            var returnValue = new { Id = testDefault, CultureTypeId = cultureType };

//            return JsonConvert.SerializeObject(returnValue);

//        }
//    }
//}
