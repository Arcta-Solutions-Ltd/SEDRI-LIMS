using arc.app.Common;
using arc.common.Models.Config;
using arc.common.Utils;
using Newtonsoft.Json;
using System.Linq;

namespace arc.app.Configuration;
internal class AddMappingValidator : ISpecialValidator
{
    private readonly string _message;

    public AddMappingValidator(string message)
    {
        _message = message;
    }
    public string ValidateMessage()
    {
        var mappingModel = JsonConvert.DeserializeObject<MappingModel>(_message, new JsonBooleanConverter());
        if (mappingModel.Mapping.Count < 1) { 
            return "@MapAddErrA@";
        }
        else if (mappingModel.Mapping.Any(p => string.IsNullOrEmpty(p.AfterMappingValue))) 
        {
            return "@MapAddErrC@";
        }
        else if (mappingModel.Mapping.Any(p => string.IsNullOrEmpty(p.BeforeMappingValue)))
        {
            return "@MapAddErrB@";
        }
        return "";
    }
}
