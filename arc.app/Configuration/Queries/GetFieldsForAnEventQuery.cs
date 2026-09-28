using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using arc.app.SystemConfig;
using Newtonsoft.Json;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryConfig;

namespace arc.app.Configuration.Queries
{
    public class GetFieldsForAnEventQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        public GetFieldsForAnEventQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var jsonConfigRepository = _serviceProvider.GetService<IJsonConfigRepository>();
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();

            var configRecords = await jsonConfigRepository.GetFormsForEventAsync(queryFilters);

            var optionList = new List<OptionsConfig>();

            foreach(var config in configRecords)
            {
                var formConfig = JsonConvert.DeserializeObject<FullFormConfig>(config.Contents);
                formConfig = await formConfigDefinition.LoadFormAsync(config.ConfigName);

                var fieldList = formConfig.GetListFieldsForForm();
                var newOptionList = fieldList.Select(f => new OptionsConfig { Key = f.Id, Text = f.Label});
                optionList.AddRange(newOptionList);
            }

            return JsonConvert.SerializeObject(optionList.GroupBy(a => a.Text).Select(b => b.First()).OrderBy(x => x.Text));
        }
    }
}
