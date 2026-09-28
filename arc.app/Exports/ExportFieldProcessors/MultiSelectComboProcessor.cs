
using System;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class MultiSelectComboProcessor
    {
        public string GetLine(ListItemProcessor listItemProcessor, string combo)
        {
            var items = combo.Split(",");
            var itemcount = 0;
            var line = "";

            foreach (var itemId in items)
            {
                if(Int32.TryParse(itemId, out int id))
                {
                    var item = listItemProcessor.GetListItem(itemId);
                    line += itemcount == 0 ? item : ", " + item;
                    itemcount++;
                }
                else
                {
                    line += combo;
                }
            }

            return line;
        }
    }
}
