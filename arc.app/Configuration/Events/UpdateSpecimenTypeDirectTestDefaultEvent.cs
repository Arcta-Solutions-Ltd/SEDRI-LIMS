//using arc.app.Common;
//using arc.app.SystemConfig;
//using arc.common;
//using arc.data.model.Configuration;
//using arc.domain.Configuration.EventsConfig;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Threading.Tasks;

//namespace arc.app.Configuration.Events;

//internal class UpdateSpecimenTypeDirectTestDefaultEvent : IRun
//{
//    private readonly IServiceProvider _serviceProvider;

//    public UpdateSpecimenTypeDirectTestDefaultEvent(IServiceProvider serviceProvider)
//    {
//        _serviceProvider = serviceProvider;
//    }

//    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//    {
//        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
//        var laboratoryConfig = await laboratoryConfigRepository.GetByIdAsync<LaboratoryConfigsDataModel>("LaboratoryConfigs", int.Parse(id));

//        if (laboratoryConfig == null)
//        {
//            var newRecord = new LaboratoryConfigsDataModel
//            {
//                ConfigName = "specimentypedirecttestdefault",
//                LaboratoryId = int.Parse(id),
//                Contents = dataToSave
//            };
//            await laboratoryConfigRepository.AddAsync(newRecord, "LaboratoryConfigs");
//        } else
//        {
//            laboratoryConfig.Contents = dataToSave;
//            await laboratoryConfigRepository.UpdateAsync(laboratoryConfig, "LaboratoryConfigs", "id");
//        }

//        return 0;
//    }
//}
