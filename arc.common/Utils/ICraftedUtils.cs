using arc.common.Models;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface ICraftedUtils
    {
        List<CraftedModel> ExtractCraftedFromJson(string message);
    }
}
