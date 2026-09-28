using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;
using System.Collections.Generic;
using arc.domain.Configuration.ListsConfig;
using arc.common.Models.Specimen;
using arc.app.Reports.InclusionSelectors;

namespace arc.app.Configuration
{
    public interface ICommentRepository
    {
        Task DeleteCommentAsync(string id);
        Task <string> GetReportCommentsAsync(QueryFilterConfig parameters);
        Task <IEnumerable<OptionsConfig>> GetCannedSpecimenCommentsAsync();
        Task<IEnumerable<OptionsConfig>> GetCannedCultureCommentsAsync();
        Task<IEnumerable<OptionsConfig>> GetCannedASTCommentsAsync();
        Task<IEnumerable<OptionsConfig>> GetCannedASTSusceptibilityOverrideCommentsAsync();
        Task<List<CommentListModel>> GetSpecimenCommentListByIdAsync(QueryFilterConfig queryFilters);
        Task<List<CommentListModel>> GetCultureCommentListByIdAsync(QueryFilterConfig queryFilters);
    }
}
