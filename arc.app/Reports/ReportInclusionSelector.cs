using arc.app.Reports.InclusionSelectors;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Reports;

public class ReportInclusionSelector : IReportInclusionSelector
{
    private readonly IDirectTestSelector _directTestSelector;
    private readonly ICultureSelector _cultureSelector;
    private readonly ICommentSelector _commentSelector;

    public ReportInclusionSelector(IDirectTestSelector directTestSelector, ICultureSelector cultureSelector, ICommentSelector commentSelector)
    {
        _directTestSelector = directTestSelector;
        _cultureSelector = cultureSelector;
        _commentSelector = commentSelector;
    }

    public async Task<string> GetFilterListsAsync(QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetIntegerValue("id");

        var returnValue = new SpecimenSelectorListModel
        {
            Id = specimenId.ToString(),
            DirectTestList = await _directTestSelector.GetContentsAsync(specimenId),
            Cultures = await _cultureSelector.GetContentsAsync(specimenId),
            Comments = await _commentSelector.GetCommentsAsync(specimenId)
        };

        return JsonConvert.SerializeObject(returnValue);
    }
}
