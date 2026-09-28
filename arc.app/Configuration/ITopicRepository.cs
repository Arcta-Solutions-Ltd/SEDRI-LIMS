using arc.common.Models.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface ITopicRepository
    {
        Task<List<TopicTranslationModel>> GetTopicsForTranslationAsync();
    }
}
