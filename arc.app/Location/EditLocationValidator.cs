using arc.app.Common;
using arc.common.Utils;
using arc.domain.Location;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Location
{
    public class EditLocationValidator : ISpecialValidatorAsync
    {
        private readonly ILocationRepository _locationRepository;
        private readonly string _message;

        public EditLocationValidator(ILocationRepository locationRepository, string message)
        {
            _locationRepository = locationRepository;
            _message = message;
        }

        public async Task<string> ValidateMessageAsync()
        {
            //var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            //var location = JsonConvert.DeserializeObject<LocationModel>(_message,settings);
            var location = ArcJson.Deserialize<LocationModel>(_message);
            if (location.ParentLocationId < 1) { return ""; }

            var locationList = await _locationRepository.GetLocationsForListAsync();

            var locationWalker = locationList.Where(o => o.Key == location.ParentLocationId.ToString()).First();
            while (locationWalker != null)
            {
                if (locationWalker.Key == location.Id.ToString()) { return "@RemCir@"; }
                if (string.IsNullOrEmpty(locationWalker.ParentKey) || locationWalker.ParentKey == "0") { return ""; }
                locationWalker = locationList.Where(o => o.Key == locationWalker.ParentKey).First();
            }
            return "";
        }
    }
}
