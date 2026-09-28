//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System;
//using System.Collections.Generic;
//using Microsoft.Extensions.DependencyInjection;
//using System.Threading.Tasks;
//using arc.app.Common;
//using arc.app.Config.Forms;
//using Newtonsoft.Json;

//namespace arc.app.Configuration.Queries
//{
//    internal class GetCultureTypeDefaultList : ISingleConfig
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IConfigExtractionUtils _configExtractionUtils;

//        internal GetCultureTypeDefaultList(IServiceProvider serviceProvider, IConfigExtractionUtils configExtractionUtils)
//        {
//            _serviceProvider = serviceProvider;
//            _configExtractionUtils = configExtractionUtils;
//        }

//        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//        {
//            var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(queryFilters.Parameters[0].Value));

//            var listRepository = _serviceProvider.GetService<IListRepository>();
//            var returnList = new List<object>();

//            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
//            if (view.DefaultCultureTests != null)
//            {
//                foreach (var def in view.DefaultCultureTests)
//                {
//                    var cultureType = await listRepository.GetValueFromIdAsync(def.CultureType);

//                    var formList = "";
//                    foreach (var form in def.Values)
//                    {
//                        var newForm = await formAdapter.GetFormAsync(form);
//                        if (newForm != null) { formList += formList == "" ? newForm.Title : ", " + newForm.Title; }
//                    }
//                    var newItem = new { CultureType = cultureType, CultureTestList = formList, Id = queryFilters.Parameters[0].Value + "|" + def.CultureType };
//                    returnList.Add(newItem);
//                }
//            }

//            return JsonConvert.SerializeObject(returnList);
//        }

//    }
//}
