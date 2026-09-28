//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using arc.app.SystemConfig;
//using arc.app.Config;
//using arc.app.Common;
//using Newtonsoft.Json;

//namespace arc.app.Configuration
//{
//    internal class GetSpecimenTypeCultureTypeList : ISingleConfig
//    {
//        private readonly IServiceProvider _serviceProvider;

//        internal GetSpecimenTypeCultureTypeList(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var configRepository = _serviceProvider.GetService<IConfigRepository>();
//            queryFilters.Parameters[0].Key = "id";
//            var viewRecord = await configRepository.SingleConfigByIdAsync(queryFilters);

//            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
//            var view = await listViewFactory.GetViewAsync(viewRecord.ConfigName);

//            var listRepository = _serviceProvider.GetService<IListRepository>();
//            var returnList = new List<object>();

//            if (view.DefaultCultures != null)
//            {
//                foreach (var culture in view.DefaultCultures)
//                {
//                    var specimenType = await listRepository.GetValueFromIdAsync(culture.SpecimenType);

//                    var cultureTypeList = "";
//                    foreach (var cultureType in culture.Values)
//                    {
//                        var cultureDesc = await listRepository.GetValueFromIdAsync(int.Parse(cultureType));

//                        cultureTypeList += cultureTypeList == "" ? cultureDesc : ", " + cultureDesc;
//                    }
//                    var newItem = new { SpecimenType = specimenType, CultureTypeList = cultureTypeList, Id = queryFilters.Parameters[0].Value + "|" + culture.SpecimenType };
//                    returnList.Add(newItem);
//                }
//            }

//            return JsonConvert.SerializeObject(returnList);
//        }
//    }
//}
