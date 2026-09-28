using arc.common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ILocalFieldResolver
    {
        Task<List<JsonFieldModel>> GetFieldValuesAsync(List<JsonFieldModel> requiredLocalFields);
    }
}
