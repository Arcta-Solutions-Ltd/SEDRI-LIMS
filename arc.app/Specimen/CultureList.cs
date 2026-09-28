using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.common.Models.Specimen;
using arc.app.Tests;
using arc.common.Models;
using arc.app.Reports;
using arc.app.AST;
using System.Linq;
using Newtonsoft.Json;
using arc.app.Common;
using arc.common.Models.Monitoring;
using arc.app.SystemConfig;

namespace arc.app.Specimen
{
    /// <summary>
    /// Builds culture list rows including aggregated completed isolate test results, with fields ordered like each test's form.
    /// </summary>
    public class CultureList : ICultureList
    {
        private readonly IServiceProvider _serviceProvider;

        public CultureList(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<CultureListModel>> GetAsync(QueryFilterConfig parameters, TokenInfoModel token)
        {
            var cultureRepository = _serviceProvider.GetService<ICultureRepository>();
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            var testListProcessor = _serviceProvider.GetService<IFormattedCultureTestList>();
            var listReplacer = _serviceProvider.GetService<IReplaceListItemValues>();
            var astRepository = _serviceProvider.GetService<IASTRepository>();
            var listFieldUtils = _serviceProvider.GetService<IListFieldUtils>();
            var testResultsForListFormatter = _serviceProvider.GetService<ITestResultsForListFormatter>();
            var configItemHandler = _serviceProvider.GetService<IConfigItemHandler>();

            var cultureList = await cultureRepository.GetCultureListBySpecimenIdAsync(parameters);
            foreach (var culture in cultureList)
            {
                var param = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = culture.Id } } };
                var cultureTests = await testRepository.GetTestsForCultureAsync(param);

                var allResults = new List<JsonItemModel>();
                var fieldOrderCache = new Dictionary<string, FormFieldOrderCache>();

                foreach (var test in cultureTests)
                {
                    if (test.Status != "Complete")
                    {
                        continue;
                    }

                    var contents = configItemHandler.GetSingleItemAsync(test.TestName).Result;
                    if (string.IsNullOrEmpty(contents))
                    {
                        continue;
                    }

                    var label = await testResultsForListFormatter.FormatCultureIsolateTestAsync(test, fieldOrderCache, applyYesNoUiAliases: true);

                    var testResultsJson = test.TestResults ?? "[]";
                    var testResultsUnfiltered = JsonConvert.DeserializeObject<List<JsonItemModel>>(testResultsJson);
                    var testResultsFiltered = testResultsUnfiltered.Where(r => r.Label != "@GenDis@").ToList();

                    var cultureTestResults = new JsonItemModel()
                    {
                        Label = label ?? test.TestName,
                        Contents = null,
                        ChildItems = testResultsFiltered
                    };
                    allResults.Add(cultureTestResults);
                }

                var astResults = await astRepository.GetAstAntibioticListAsync(param);

                var diskResults = astResults.Where(r => r.TestMethodId == 681 && r.ExpertRuleId == 0);
                var micResults = astResults.Where(r => r.TestMethodId == 680 && r.ExpertRuleId == 0);
                var expertRuleResults = astResults.Where(r => r.ExpertRuleId != 0);

                var diskResultsToAdd = diskResults.Select(a => new JsonItemModel { Label = a.Antibiotic, Contents = a.Susceptibility + (!string.IsNullOrEmpty(a.SpecialConsideration) ? " (" + a.SpecialConsideration + ")" : "") }).ToList();
                if (diskResultsToAdd.Count > 0)
                {
                    allResults.Add(new JsonItemModel()
                    {
                        Label = "@GenDisC@",
                        Contents = null,
                        ChildItems = diskResultsToAdd
                    });
                }

                var micResultsToAdd = micResults.Select(a => new JsonItemModel { Label = a.Antibiotic, Contents = a.Susceptibility + (!string.IsNullOrEmpty(a.SpecialConsideration) ? " (" + a.SpecialConsideration + ")" : "") }).ToList();
                if (micResultsToAdd.Count > 0)
                {
                    allResults.Add(new JsonItemModel()
                    {
                        Label = "@AstMicA@",
                        Contents = null,
                        ChildItems = micResultsToAdd
                    });
                }

                var ruleResultsToAdd = expertRuleResults.Select(a => new JsonItemModel { Label = a.Antibiotic, Contents = a.Susceptibility + (!string.IsNullOrEmpty(a.SpecialConsideration) ? " (" + a.SpecialConsideration + ")" : "") }).ToList();
                if (ruleResultsToAdd.Count > 0)
                {
                    allResults.Add(new JsonItemModel()
                    {
                        Label = "@GenRulA@",
                        Contents = null,
                        ChildItems = ruleResultsToAdd
                    });
                }

                culture.TestResults = JsonConvert.SerializeObject(allResults);
            }
            return cultureList;
        }
    }
}
