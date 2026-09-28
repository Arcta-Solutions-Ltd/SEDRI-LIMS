using arc.app.Common;
using arc.app.Config;
using arc.app.Laboratory;
using arc.common;
using arc.common.Models;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen
{
    public class Day1BenchReadEvent : IRun
    {
        private readonly IListViewConfigFactory _listViewConfigFactory;
        private readonly ICultureRepository _cultureRepository;
        private readonly IServiceProvider _serviceProvider;
        public TokenInfoModel _token;

        public Day1BenchReadEvent(IListViewConfigFactory listViewConfigFactory, ICultureRepository cultureRepository, TokenInfoModel token, IServiceProvider serviceProvider)
        {
            _listViewConfigFactory = listViewConfigFactory;
            _cultureRepository = cultureRepository;
            _token = token;
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string message, string Id, EventModel command, EventConfig eventData = null)
        {
            command.Event = "addculture";
            var day1BenchRead = JsonConvert.DeserializeObject<Day1BenchReadEventModel>(message.Replace("\"", "'"));

            var specimenQuantityId = day1BenchRead.BenchReadDay1Action == "593" ? 177 : day1BenchRead.BenchReadDay1Action == "594" ? 1087 : 0;
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("SpecimenId", Id);
            var cultureList = await _cultureRepository.GetCultureListBySpecimenIdAsync(queryFilter);
            string username = _token.Username;

            if (cultureList.Count() == 0)
            {
                var viewConfig = await _listViewConfigFactory.GetViewAsync(day1BenchRead.View);

                // Load laboratory configuration for culture test defaults
                var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
                await laboratoryConfigurationHandler.LoadConfigurationForSpecimenAsync(int.Parse(Id));

                var newMessage = specimenQuantityId == 0 ? @"{'SpecimenId':'" + Id + "','TypeId':'978'}" : @"{'SpecimenId':'" + Id + "','SpecimenQuantityId':'" + specimenQuantityId + "','TypeId':'978', 'DisplayOnReport': 'Yes'}";
                await _cultureRepository.AddCultureAsync(newMessage, command, eventData, laboratoryConfigurationHandler.LaboratoryConfigurationList, username);
            } else
            {
                if (day1BenchRead.BenchReadDay1Action != "595")
                {
                    await _cultureRepository.GrowthForAllCulturesInSpecimenAsync(cultureList, specimenQuantityId, int.Parse(command.NewStateId), int.Parse(Id));
                }
            }

            return int.Parse(Id);
        }
    }
}
