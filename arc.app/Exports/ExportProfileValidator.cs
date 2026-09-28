using arc.app.Common;
using arc.common.Models.Export;
using arc.common.Utils;
using Newtonsoft.Json;

namespace arc.app.Exports
{
    internal class ExportProfileValidator : ISpecialValidator
    {
        private readonly string _message;

        public ExportProfileValidator(string message)
        {
            _message = message;
        }
        public string ValidateMessage()
        {
            var exProf = JsonConvert.DeserializeObject<ExportProfileModel>(_message, new JsonBooleanConverter());
            if (string.IsNullOrEmpty(exProf.Name))
            {
                return "@ExpProNErr@";
            }
            else if (string.IsNullOrEmpty(exProf.Description))
            {
                return "@ExpProDErr@";
            }
            return "";
        }
    }
}
