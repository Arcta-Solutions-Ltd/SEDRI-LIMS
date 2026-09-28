//using arc.app.Common;
//using arc.common;
//using arc.common.Models.Export;
//using arc.common.Utils;
//using arc.domain.Configuration.EventsConfig;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using System;
//using System.Threading.Tasks;

//namespace arc.app.Import
//{
//    internal class AddImportProfileEvent : IRun
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public AddImportProfileEvent(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }
//        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
//        {
//            var importProfile = _serviceProvider.GetService<IImportRepository>();
//            var data = JsonConvert.DeserializeObject<ExportProfileModel>(dataToSave, new JsonBooleanConverter());
//            return await importProfile.AddImportProfileAsync(data);
//        }
//    }
//}
