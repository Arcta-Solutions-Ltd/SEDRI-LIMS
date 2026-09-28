using arc.common.Models;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IConvertJsonStructureToKeyValuePair
    {
        IEnumerable<KeyValueModel> Convert(string json, bool removeId);
    }
}
