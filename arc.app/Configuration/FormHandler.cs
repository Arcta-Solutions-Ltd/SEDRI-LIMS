using arc.app.Config.Forms;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class FormHandler : IFormHandler
    {
        private readonly IFormConfigAdapter _formAdapter;
        private readonly IConfigRepository _configRepository;

        public FormHandler(IFormConfigAdapter formAdapter, IConfigRepository configRepository)
        {
            _formAdapter = formAdapter;
            _configRepository = configRepository;
        }

        public async Task<string> GetFormListAsync(bool testonly = false, string testType = "tests", TokenInfoModel token = null)
        {
            var internalFormList = new[] {
                new { 
                    view = "specimen", 
                    forms = new List<string> { "ackreceiptform", "addcultureform","ast", "createspecimenreceivedform", "createspecimenrequestform",
                        "culturelistform","editpatientcollectionform","rejectspecimenform","submitconfirmationform"}
                },
                new {
                    view = "patient",
                    forms = new List<string> {"editpatientform" }
                },
                new {
                    view = "role",
                    forms = new List<string> { "addroleform", "cloneroleform", "deleteroleform","editroleform","eventpermissions", "menupermissions" }
                },
                new {
                    view = "user",
                    forms = new List<string> { "adduserform", "edituserform" }
                }
            }.ToList();

            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigTypeId", Value = "1" } } };
            var testList = await _configRepository.GetConfigListAsync(parameters);
            internalFormList.Add(new { view = "tests", forms = testList.Select(t => t.ConfigName).ToList() });

            parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigTypeId", Value = "2" } } };
            testList = await _configRepository.GetConfigListAsync(parameters);
            internalFormList.Add(new { view = "culturetests", forms = testList.Select(t => t.ConfigName).ToList() });


            var returnList = new List<FormListModel>();

            foreach (var forms in internalFormList)
            {
                if ((testonly && forms.view == testType) || (! testonly && forms.view != testType))
                {
                    foreach (var form in forms.forms)
                    {
                        var newItem = await _formAdapter.GetFormAsync(form);
                        var newForm = new FormListModel { Name = newItem.Name, Title = newItem.Title, View = forms.view };
                        returnList.Add(newForm);
                    }
                }
            }

            var list = returnList.OrderBy(o => o.Name).ToList();

            return JsonConvert.SerializeObject(list);
        }

    }
}
