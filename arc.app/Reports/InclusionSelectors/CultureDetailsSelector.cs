using arc.app.AST;
using arc.app.Config.Forms;
using arc.app.Configuration;
using arc.app.Tests;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    public class CultureDetailsSelector : ICultureDetailsSelector
    {
        private readonly ITestRepository _testRepository;
        private readonly IASTRepository _astRepository;
        private readonly IFormConfigAdapter _formConfigAdapter;
        private readonly ICommentRepository _commentRepository;

        public CultureDetailsSelector(ITestRepository testRepository, IASTRepository astRepository, IFormConfigAdapter formConfigAdapter, ICommentRepository commentRepository)
        {
            _testRepository = testRepository;
            _astRepository = astRepository;
            _formConfigAdapter = formConfigAdapter;
            _commentRepository = commentRepository;
        }

        public async Task<CultureDetailsSelectorModel> GetContentsAsync(QueryFilterConfig queryFilters)
        {
            var idRecord = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var cultureId = idRecord.Value;

            return new CultureDetailsSelectorModel
            {
                Id = cultureId,
                CultureTestGrid = await GetCultureTestContents(cultureId),
                AstGrid = await GetAstContents(cultureId),
                CultureCommentGrid = await GetCultureComments(cultureId)
            };
        }

        private async Task<List<CultureDetailsTest>> GetCultureTestContents(string cultureId)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = cultureId } } };
            var cultureTestList = await _testRepository.GetTestsForCultureAsync(queryFilters);

            var contents = new List<CultureDetailsTest>();
            foreach (var item in cultureTestList)
            {
                if (item.TestResults != null)
                {
                    var testContents = JsonConvert.DeserializeObject<PrintOnReportModel>(item.TestResults);
                    var form = await _formConfigAdapter.GetFormAsync(item.TestName);
                    var newEntry = new CultureDetailsTest { Id = item.Id.ToString(), CultureTest = form.Title, PrintOnReport = testContents.PrintOnReport };
                    contents.Add(newEntry);
                }
            }
            return contents.ToList();
        }

        /// <summary>
        /// Loads AST rows for the culture print selector. Parent and special consideration rows use separate
        /// display-on-report storage; row type is determined by special consideration id, not translated labels.
        /// </summary>
        /// <param name="cultureId">Culture id for the selector form.</param>
        /// <returns>AST grid rows with effective PrintOnReport and SpecialConsiderationId for id-based save.</returns>
        private async Task<List<CultureDetailsAst>> GetAstContents(string cultureId)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = cultureId } } };
            var astList = await _astRepository.GetAstAntibioticListAsync(queryFilters);

            var contents = astList.Select(t =>
            {
                var isSpecial = AntibioticListForReportExtensions.IsSpecialConsiderationReportRow(t.SpecialConsiderationId);
                var susceptibilityLabel = string.IsNullOrEmpty(t.Susceptibility) ? string.Empty : " (" + t.Susceptibility + ")";
                var astTest = t.Antibiotic + susceptibilityLabel;
                if (isSpecial && !string.IsNullOrEmpty(t.SpecialConsideration))
                {
                    astTest += " — " + t.SpecialConsideration;
                }

                return new CultureDetailsAst
                {
                    Id = t.Id,
                    SpecialConsiderationId = t.SpecialConsiderationId,
                    AstTest = astTest,
                    PrintOnReport = t.ResolveDisplayOnReportForReport()
                };
            });

            return contents.ToList();
        }

        private async Task<List<CultureComments>> GetCultureComments(string cultureId)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = cultureId } } };
            var cultureComments = await _commentRepository.GetCultureCommentListByIdAsync(queryFilters);
            var returnList = new List<CultureComments>();

            foreach (var comment in cultureComments)
            {
                var newModel = new CultureComments
                {
                    Id = comment.Id,
                    Comment = comment.Comment,
                    PrintOnReport = comment.DisplayOnReport,
                    CommentType = comment.CommentType,
                    Username = comment.AddedBy
                };
                returnList.Add(newModel);
            }

            return returnList;
        }
    }
}
