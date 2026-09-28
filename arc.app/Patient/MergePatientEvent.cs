using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Patient
{
    internal class MergePatientEvent : IRun
    {
        public IServiceProvider _serviceProvider;

        public MergePatientEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var patientRepository = _serviceProvider.GetService<IPatientRepository>();
            return await patientRepository.MergePatientAsync(dataToSave);
        }
    }
}
