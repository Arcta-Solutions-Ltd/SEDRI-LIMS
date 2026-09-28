using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

public interface IExpertRuleHandler
{
    Task<string> GetExpertRuleAsync(QueryFilterConfig queryFilters);
}
