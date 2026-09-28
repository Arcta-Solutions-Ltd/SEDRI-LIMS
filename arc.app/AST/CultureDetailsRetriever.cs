using arc.app.Specimen;
using arc.app.Tests;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public class CultureDetailsRetriever
    {
        private readonly ICultureRepository _cultureRepository;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly ITestRepository _testRepository;

        public CultureDetailsRetriever(ICultureRepository cultureRepository, ISpecimenRepository specimenRepository, ITestRepository testRepository)
        {
            _cultureRepository = cultureRepository;
            _specimenRepository = specimenRepository;
            _testRepository = testRepository;
        }

        /// <summary>
        /// Retrieves AST culture details for the given culture. The returned CultureTests include Id for workflow and form loading.
        /// </summary>
        public async Task<ASTCultureModel> Get(string cultureId)
        {
            var cultureResult = await _cultureRepository.GetASTCultureDataAsync(cultureId);
            var cultureTestResults = await _testRepository.GetTestsForCultureAsync(cultureId);
            var specimenTypeId = await _specimenRepository.GetSpecimenTypeAsync(cultureResult.SpecimenId);

            var cultureTests = cultureTestResults
                .Select(t => new CultureTestStatusModel { Id = t.Id, TestName = t.TestName, Status = t.Status ?? "" })
                .ToList();

            var cultureDetails = new ASTCultureModel
            {
                CultureTests = cultureTests,
                ASTCommentOne = cultureResult.ASTCommentOne,
                ASTCommentOneId = cultureResult.ASTCommentOneId,
                ASTCommentTwo = cultureResult.ASTCommentTwo,
                ASTCommentTwoId = cultureResult.ASTCommentTwoId,
                ASTAdditionalNotes = cultureResult.ASTAdditionalNotes,
                CompletedDate = cultureResult.CompletedDate,
                CompletedTime = cultureResult.CompletedTime,
                SpecimenTypeId = specimenTypeId,
                OrganismId = cultureResult.OrganismId,
                OrgGroupCodingId = cultureResult.OrgGroupCodingId,
                CultureTypeId = cultureResult.CultureTypeId,
                LaboratoryId = cultureResult.LaboratoryId
            };
            return cultureDetails;
        }
    }
}
