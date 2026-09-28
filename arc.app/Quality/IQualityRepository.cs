using arc.common.Models.Quality;
using arc.common.Models.QualityAssurance;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public interface IQualityRepository
    {
        Task<List<IqcTestProfileListModel>> GetIqcTestProfileListQueryAsync(QueryFilterConfig queryFilters);
        Task<IqcTestProfileListModel> GetIqcTestProfileSingleQueryAsync(QueryFilterConfig queryFilters);
        Task<List<OrganismAntibioticModel>> GetQcAntibioticsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilters);
        Task<List<IqcTestProfileQcOrganismsModel>> GetQcOrganismsForIqcTestAsync(QueryFilterConfig queryFilters);
        Task<List<IqcResultGridViewModel>> GetIqcTestResultsAsync(QueryFilterConfig queryFilters);
        Task<int> EditIqcTestProfileAsync(QualityCraftedModel data);
        Task<QcOrganism> GetQcOrganismByIdAsync(QueryFilterConfig queryFilters);
        Task<int> AddIqcTestAsync(AddIqcTestModel addIqcTestModel);
        Task<int> EditIqcTestQcOrganismsAsync(string dataToSave);
        Task<int> EditIqcResultAsync(IqcTest iqcTest);
        Task<int> RunIqcTestAsync(IqcTest iqcTest);
        Task<EditIqcTestModel> RunIqcTestQueryAsync(QueryFilterConfig queryFilters);
        Task DeleteIqcTestProfileAsync(string listItemId);
        Task<bool> IsIqcTestProfileOrganismUsedByDefaultAsync(QueryFilterConfig queryFilters);
        Task<List<IqcTestProfileQcAntibioticTableRow>> GetMicAntibioticsDetailsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilterConfig);
        Task<List<IqcTestProfileQcAntibioticTableRow>> GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilterConfig);
        Task<int> AddIqcTestProfileAsync(IqcTestProfile iqcTestProfile);
        Task<List<QcOrganism>> GetAllQcOrganismsByTestMethodWithChildrenAsync(QueryFilterConfig queryFilterConfig);
        Task<IqcTestProfile> GetIqcTestProfileByNameAsync(QueryFilterConfig queryFilterConfig);
        Task<IqcTestProfile> GetIqcTestProfileByIdAsync(QueryFilterConfig queryFilterConfig);
        Task<EditIqcTestModel> EditIqcResultQueryAsync(QueryFilterConfig queryFilterConfig);
        Task<IqcTest> GetIqcTestByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<IqcTestListModel>> GetIqcTestsListQueryAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetIqcTestProfileNamesListAsync();
        Task MarkIqcTestCompleteAsync(int iqcTestId, int completedListItemId);
        Task<int> GetIqcTestIdFromIqcResultIdAsync(int iqcResultId);
    }
}
