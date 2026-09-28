//using arc.app.Common;
//using arc.common.Models;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using System;
//using System.Threading.Tasks;

//namespace arc.app.Import
//{
//    internal class EditImportProfileQuery : IQueryRun
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public EditImportProfileQuery(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
//        {
//            var importRepository = _serviceProvider.GetService<IImportRepository>();
//            var importList = await importRepository.EditImportProfileQueryAsync(queryFilter);

//            return JsonConvert.SerializeObject(importList);
//        }
//    }
//}
