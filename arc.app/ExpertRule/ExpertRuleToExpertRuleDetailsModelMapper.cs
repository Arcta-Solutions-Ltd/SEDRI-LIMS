using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.domain.Coding;
using System.Linq;

namespace arc.app.ExpertRule;

/// <summary>
/// Maps the ExpertRule type onto the ExpertRuleDetailsModel type.
/// </summary>
public class ExpertRuleToExpertRuleDetailsModelMapper : IMapType<arc.domain.Coding.ExpertRule, ExpertRuleDetailsModel>
{
    /// <summary>
    /// Maps a <see cref="arc.domain.Coding.ExpertRule"/> object to a <see cref="ExpertRuleDetailsModel"/> object.
    /// </summary>
    /// <param name="grid">The <see cref="arc.domain.Coding.ExpertRule"/> object to map.</param>
    /// <returns>The mapped <see cref="ExpertRuleDetailsModel"/> object.</returns>
    public ExpertRuleDetailsModel Map(arc.domain.Coding.ExpertRule source)
    {
        var result = source.Map<ExpertRuleDetailsModel>();
        return result;
    }
}
