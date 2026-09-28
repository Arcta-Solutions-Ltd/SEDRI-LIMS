using arc.app.Common;
using arc.common;
using arc.common.Data;
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
    internal class AddExportProfileFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddExportProfileFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var exportProfileField = _serviceProvider.GetService<IExportProfileFieldRepository>();
            var moreDataGenerator = _serviceProvider.GetService<IGenerateMoreData>();
            var jsonRemover = _serviceProvider.GetService<IJsonElementRemover>();

            var data = GetExportProfileFileData(dataToSave, moreDataGenerator, jsonRemover);
            var queryFilter = new domain.Configuration.QueryFiltersConfig.QueryFilterConfig
            {
                Parameters = new List<domain.Configuration.QueryFiltersConfig.QueryValuesConfig>
                {
                    new domain.Configuration.QueryFiltersConfig.QueryValuesConfig
                    {
                        Key = "Id",
                        Value = data.ExportProfileId.ToString()
                    }
                }
            };
            var existingFields = await exportProfileField.GetByProfileIdAsync(queryFilter);
            if (existingFields.Count() > 0)
            {
                var highestOrderNumber = existingFields.OrderByDescending(a => a.OrderNumber).First().OrderNumber;
                data.OrderNumber = highestOrderNumber + 1;
            }

            return await exportProfileField.AddExportProfileFieldAsync(data);
        }

        private ExportProfileFieldModel GetExportProfileFileData(string dataToSave, IGenerateMoreData moreDataGenerator, IJsonElementRemover jsonRemover)
        {
            var vm = JsonConvert.DeserializeObject<AddExportProfileFieldViewModel>(dataToSave);
            var names = vm.Name.Split('|');

            if (names[0] == "WhonetAntibiotic")
            {
                vm.Heading = "<:WHONET:>";
            }

            dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "Name");
            dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "Heading");
            var moreData = moreDataGenerator.GetMoreDataJsonString("exportprofilerecord", dataToSave);

            return new ExportProfileFieldModel
            {
                ExportProfileId = vm.ExportProfileId,
                FieldName = names[0],
                FormName = names[1],
                TableName = names[2],
                LabelName = names[3],
                ModifiedDate = DateTime.UtcNow,
                HeaderName = vm.Heading,
                OrderNumber = 1,
                MoreData = moreData
            };
        }
    }
}
