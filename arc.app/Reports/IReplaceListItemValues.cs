using arc.common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports;

public interface IReplaceListItemValues
{
    Task<List<KeyValueModel>> ReplaceInStandardListAsync(List<KeyValueModel> currentItems, List<string> itemsToChange);
    Task<List<string>> ReplaceInGridAsync(List<string> currentLines, List<bool> itemsToChange);
}
