using arc.common.Models.Reports;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public interface IConvertJsonStructureToPipeSeparatedList
    {
        List<GridLineContentsModel> Convert(string json);
    }
}
