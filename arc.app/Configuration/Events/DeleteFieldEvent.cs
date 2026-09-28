using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.SystemConfig;
using Newtonsoft.Json;

namespace arc.app.Configuration.Events
{
    internal class DeleteFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {


            var idModel = JsonConvert.DeserializeObject<IdModel>(dataToSave);
            var idList = idModel.Id.Split("|");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var fieldConfigUtils = _serviceProvider.GetService<IFieldConfigUtils>();

            var formToUpdate = await formConfigDefinition.LoadFormAsync(idList[0]);

            //formToUpdate.DeleteField(idList[2]);
            formToUpdate = await fieldConfigUtils.DeleteFieldFromSystemAsync(idList[2], formToUpdate);

            await formConfigRepository.UpdateFormAsync(formToUpdate);

            return 0;
        }
    }
}
