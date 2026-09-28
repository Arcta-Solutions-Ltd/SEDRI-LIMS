//using arc.domain.Configuration.QueryConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using arc.app.Common;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using arc.app.Config.Forms;
//using arc.app.SystemConfig;
//using arc.common.Utils;
//using arc.common.Models.Config;

//namespace arc.app.Configuration;

//internal class GetSpecimenTypeDefaultList : ISingleConfig
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly string _configName;

//    internal GetSpecimenTypeDefaultList(string configName,IServiceProvider serviceProvider)
//    {
//        _serviceProvider = serviceProvider;
//        _configName = configName;
//    }

//    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
//    {
//        queryFilters.AddString("configname", _configName);
//        queryFilters.Parameters[0].Key = "id";

//        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
//        var configList = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilters);
                     
//        var listRepository = _serviceProvider.GetService<IListRepository>();
//        var returnList = new List<object>();

//        var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
//        if (configList != null)
//        {
//            foreach (var def in configList)
//            {
//                var defValues = ArcJson.Deserialize<DirectTestDefaultModel>(def.Contents);
//                var specimenType = await listRepository.GetValueFromIdAsync(defValues.SpecimenTypeId);

//                var formList = "";
//                foreach(var form in defValues.DirectTestId.Split(","))
//                {
//                    var newForm = await formAdapter.GetFormAsync(form);
//                    if(newForm != null){ formList += formList == "" ? newForm.Title : ", " + newForm.Title; }
//                }
//                var newItem = new { SpecimenType = specimenType, DirectTestList = formList, def.Id };
//                returnList.Add(newItem);
//            }
//        }

//        return JsonConvert.SerializeObject(returnList);
//    }
//}
