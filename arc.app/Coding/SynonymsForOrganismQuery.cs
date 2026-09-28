using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.common.Models.Coding;
using Newtonsoft.Json;
using System.Linq;

namespace arc.app.Coding
{
    internal class SynonymsForOrganismQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public SynonymsForOrganismQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
        {
            var organismRepository = _serviceProvider.GetService<IOrganismRepository>();

            var synonymList = await organismRepository.GetOrganismSynonymListAsync(queryFilter);

            var returnResult = new EditSynonymModel { Id = 0, PreferredName = "" };

            if (synonymList.Count() > 0)
            {
                returnResult.Id = synonymList.First().OrganismId;
                var preferredNameRecord = synonymList.FirstOrDefault(f => f.PreferredName == true);
                if (preferredNameRecord != null)
                {
                    returnResult.PreferredName = preferredNameRecord.Synonym;
                }

                returnResult.SynonymGrid = synonymList.Where(f => !f.PreferredName).Select(f => new SynonymModel { Synonym = f.Synonym }).ToList();
            }

            return JsonConvert.SerializeObject(returnResult);
        }
    }
}
