using arc.app.Common;
using arc.common;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    internal class EditExportProfileFieldsEvent:IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditExportProfileFieldsEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var profileFieldRepo = _serviceProvider.GetRequiredService<IExportProfileFieldRepository>();
            var queryFilter = new domain.Configuration.QueryFiltersConfig.QueryFilterConfig {
                Parameters = new List<domain.Configuration.QueryFiltersConfig.QueryValuesConfig>
                { 
                    new domain.Configuration.QueryFiltersConfig.QueryValuesConfig
                    { 
                        Key = "Id",
                        Value = Id
                    }
                }
                };

            var fields = await profileFieldRepo.GetByProfileIdAsync(queryFilter);
            var updateVm = JsonConvert.DeserializeObject<UpdateExportProfileFieldsViewModel>(dataToSave);
            var orderTrack = 1;
            foreach (var field in updateVm.FieldList)
            {
               var fToUpdate = fields.FirstOrDefault(a => a.FieldName.Is(field.Value));
                fToUpdate.OrderNumber = orderTrack;

                await profileFieldRepo.UpdateAsync(fToUpdate.Id.ToString(), fToUpdate);
                ++orderTrack;

            }
            return 1;
        }
    }
}
