using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Coding
{
    internal class DeleteTestPatternEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteTestPatternEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var testPatternRepository = _serviceProvider.GetService<ITestPatternRepository>();
            await testPatternRepository.DeleteTestPatternAsync(id);

            return int.Parse(id);
        }
    }
}
