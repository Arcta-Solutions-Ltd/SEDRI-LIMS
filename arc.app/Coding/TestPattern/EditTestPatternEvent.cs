using System;
using System.Threading.Tasks;
using arc.app.Common;
using arc.common;
using arc.common.Models.Coding;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;

namespace arc.app.Coding
{
    internal class EditTestPatternEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditTestPatternEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var testPatternRepository = _serviceProvider.GetService<ITestPatternRepository>();
            var jsonElementRemover = _serviceProvider.GetService<IJsonElementRemover>();

            dataToSave = jsonElementRemover.RemoveElementsByValue(dataToSave, "crafted");
            var testPatternModel = ArcJson.Deserialize<TestPatternModel>(dataToSave);
            return await testPatternRepository.EditTestPatternAsync(testPatternModel);
        }
    }
}
