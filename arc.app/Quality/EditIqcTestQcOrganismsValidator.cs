using arc.app.Common;
using arc.common.Models.Role;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace arc.app.Quality
{
    internal class EditIqcTestQcOrganismsValidator : ISpecialValidator
    {
        private readonly string _message;

        public EditIqcTestQcOrganismsValidator(string message)
        {
            _message = message;
        }

        public string ValidateMessage()
        {
            var data = JsonConvert.DeserializeObject<dynamic>(_message);
            JArray toggles = data.Crafted[0].Contents;

            var allowedCount = toggles
                .Select(x => x.ToObject<CraftedSelectionsModel>())
                .Where(x => x.Allowed == "Yes")
                .Select(x => int.Parse(x.Key))
                .Count();

            return allowedCount > 0 ? "" : "@QuaIqcTesB@";
        }
    }
}
