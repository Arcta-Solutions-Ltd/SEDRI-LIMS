using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public class PreferenceHandler : IPreferenceHandler
    {
        private readonly IUserRepository _userRepository;

        public PreferenceHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            queryFilters.AddInteger("id", int.Parse(token.Id));
            var result = await _userRepository.PreferenceByIdAsync(queryFilters);
            return JsonConvert.SerializeObject(result);
        }

    }
}
