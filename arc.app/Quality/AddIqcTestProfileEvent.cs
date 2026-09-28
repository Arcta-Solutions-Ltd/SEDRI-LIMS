using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class AddIqcTestProfileEvent : IRun
    {
        private readonly IQualityRepository _qualityRepository;
        private readonly IListRepository _listRepository;

        public AddIqcTestProfileEvent(IServiceProvider serviceProvider)
        {
            _qualityRepository = serviceProvider.GetService<IQualityRepository>();
            _listRepository = serviceProvider.GetService<IListRepository>();
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var queryFilterConfig = new QueryFilterConfig();
            var data = JsonConvert.DeserializeObject<dynamic>(dataToSave);

            queryFilterConfig.AddString("id", data.TestMethodId.Value);
            var testMethodListItem = await _listRepository.GetListItemByIdAsync(queryFilterConfig);

            var iqcTestProfile = new IqcTestProfile()
            {
                Name = data.Name,
                TestMethodListItemId = testMethodListItem.Id
            };

            queryFilterConfig.AddString("testmethod", testMethodListItem.Value);

            var qcOrganisms = await _qualityRepository.GetAllQcOrganismsByTestMethodWithChildrenAsync(queryFilterConfig);

            return await _qualityRepository.AddIqcTestProfileAsync(AddQcOrganismsToIqcTestProfile(iqcTestProfile, qcOrganisms));
        }

        private IqcTestProfile AddQcOrganismsToIqcTestProfile(IqcTestProfile iqcTestProfile, IEnumerable<QcOrganism> qcOrganisms)
        {
            foreach (var qcOrganism in qcOrganisms)
            {
                var iqcTestProfileQcOrganism = new IqcTestProfileQcOrganism()
                {
                    QcOrganismId = qcOrganism.Id
                };

                foreach (var qcAntibiotic in qcOrganism.QcAntibiotics)
                {
                    iqcTestProfileQcOrganism.IqcTestProfileQcAntibiotics.Add(
                        new IqcTestProfileQcAntibiotic()
                        {
                            QcAntibioticId = qcAntibiotic.Id,
                            Enabled = false
                        }
                   );
                }
                iqcTestProfile.IqcTestProfileQcOrganisms.Add(iqcTestProfileQcOrganism);
            }
            return iqcTestProfile;
        }
    }
}
