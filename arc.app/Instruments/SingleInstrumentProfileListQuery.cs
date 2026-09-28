using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.SystemConfig;
using arc.app.Config.Forms;
using arc.app.Config.Events;
using Newtonsoft.Json;
using arc.domain.Instruments;
using arc.common.Models.Instruments;

namespace arc.app.Instruments
{
    internal class SingleInstrumentProfileListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        internal SingleInstrumentProfileListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var listRepository = _serviceProvider.GetService<IListRepository>();
            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
            var eventAdapter = _serviceProvider.GetService<IEventAdapter>();

            var filter = new QueryFilterConfig();
            filter.AddString("configname", "Instrumentinfo");
            var instrumentConfig = await configRepository.SingleConfigByNameAsync(filter);

            var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);

            var returnValue = new InstrumentProfileListModel();

            foreach (var item in fullConfig.Instruments)
            {
                if (item.InstrumentName == queryFilter.Parameters[0].Value)
                {
                    var cultureType = "";
                    if (!string.IsNullOrEmpty(item.CultureTypeId)
                        && int.TryParse(item.CultureTypeId.Trim(), out var cultureTypeListId))
                    {
                        cultureType = await listRepository.GetValueFromIdAsync(cultureTypeListId);
                    }

                    var specimenType = "";
                    if (!string.IsNullOrEmpty(item.SpecimenTypeId))
                    {
                        var listOfItems = item.SpecimenTypeId.Split(",");
                        foreach (var listItem in listOfItems)
                        {
                            var itemString = await listRepository.GetValueFromIdAsync(int.Parse(listItem));
                            specimenType += specimenType == "" ? itemString : ", " + itemString;
                        }
                    }

                    var directTest = "";
                    if (!string.IsNullOrEmpty(item.DirectTestId))
                    {
                        var newForm = await formAdapter.GetFormAsync(item.DirectTestId);
                        var formEvent = await eventAdapter.GetEventAsync(newForm.SaveEvent);
                        directTest = formEvent.Description;
                    }

                    var cultureTest = "";
                    if (!string.IsNullOrEmpty(item.CultureTestId))
                    {
                        var newForm = await formAdapter.GetFormAsync(item.CultureTestId);
                        var formEvent = await eventAdapter.GetEventAsync(newForm.SaveEvent);
                        cultureTest = formEvent.Description;
                    }

                    var organismGroup = "";
                    if (!string.IsNullOrEmpty(item.ConfigListId))
                    {
                        organismGroup = await listRepository.GetValueFromIdAsync(int.Parse(item.ConfigListId));
                    }

                    var instrumentMachine = "";
                    if (!string.IsNullOrWhiteSpace(item.InstrumentMachineId) && int.TryParse(item.InstrumentMachineId.Trim(), out var machineListItemId))
                    {
                        instrumentMachine = await listRepository.GetValueFromIdAsync(machineListItemId);
                    }

                    var newListItem = new InstrumentProfileListModel { CultureType = cultureType, SpecimenType = specimenType, InstrumentName = item.InstrumentName, InstrumentMachine = instrumentMachine, DirectTest = directTest, CultureTest = cultureTest, OrganismGroup = organismGroup, Id = item.InstrumentName };
                    returnValue = newListItem;
                }
            }

            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
