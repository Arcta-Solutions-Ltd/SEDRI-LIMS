using arc.app.Common;
using arc.common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports;

public class ReplaceListItemValues : IReplaceListItemValues
{
    private readonly IListRepository _listRepository;

    public ReplaceListItemValues(IListRepository listRepository)
    {
        _listRepository = listRepository;
    }

    public async Task<List<KeyValueModel>> ReplaceInStandardListAsync(List<KeyValueModel> currentItems, List<string> itemsToChange)
    {
        var items = itemsToChange.ConvertAll(d => d.ToLower());
        foreach (var item in currentItems)
        {
            if (items.Contains(item.Key.ToLower()))
            {
                var itemList = item.Value.Split(",");
                var valueToChangeTo = "";
                foreach (var el in itemList)
                {
                    var newValue = await _listRepository.GetValueFromIdAsync(int.Parse(el));
                    valueToChangeTo = string.IsNullOrEmpty(valueToChangeTo) ? newValue : valueToChangeTo + ", " + newValue;
                }
                item.Value = valueToChangeTo;
            }
        }

        return currentItems;
    }

    public async Task<List<string>> ReplaceInGridAsync(List<string> currentLines, List<bool> itemsToChange)
    {
        var newgrid = new List<string>();
        foreach (var item in currentLines)
        {
            var items = item.Split("|");
            var count = 0;
            var newLine = "";
            foreach (var el in items)
            {
                if (!string.IsNullOrEmpty(el))
                {
                    var newValue = "";
                    if (!itemsToChange[count])
                    {
                        newValue = el;
                    }
                    else if (el.Contains(","))
                    {
                        var listItemValues = new List<string>();
                        foreach (var listItemId in el.Split(",").Select(x => int.Parse(x)))
                        {
                            listItemValues.Add(await _listRepository.GetValueFromIdAsync(listItemId));
                        }
                        newValue = string.Join(",", listItemValues);
                    }
                    else
                    {
                        newValue = await _listRepository.GetValueFromIdAsync(int.Parse(el));
                    }

                    newLine += newLine == "" ? newValue : "|" + newValue;
                }
                count++;
            }
            newgrid.Add(newLine);
        }

        return newgrid;
    }
}
