using arc.app.Common;
using arc.common;
using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Quality;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class EditIqcResultEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditIqcResultEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var jsonObject = JObject.Parse(dataToSave);

            if (jsonObject.SelectToken("Crafted[0].Contents[1].value") == null)
            {
                return (int)jsonObject["Id"];
            }

            var iqcTest = new IqcTest()
            {
                Id = (int)jsonObject["Crafted"][0]["Contents"][0]["value"],
                Results = new List<IqcResult>()
            };

            var organisms = JsonConvert.DeserializeObject<List<QcOrganismWithIqcResultsModel>>(jsonObject["Crafted"][0]["Contents"][1]["value"].ToString());

            foreach (var organism in organisms)
            {
                foreach (var result in organism.IqcResults)
                {
                    iqcTest.Results.Add(new IqcResult()
                    {
                        Id = result.IqcResultId,
                        TestId = iqcTest.Id,
                        Value = result.ResultValue
                    });
                }
            }

            return await _serviceProvider.GetService<IQualityRepository>().EditIqcResultAsync(iqcTest);
        }
    }
}
