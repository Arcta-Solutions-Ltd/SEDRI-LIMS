using arc.common.Utils;
using arc.domain.Configuration.PagesConfig;
using System.Linq;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class GridProcessor
    {

        public string GetLine(ListItemProcessor listItemProcessor, MultiSelectComboProcessor multiSelectComboProcessor, IJsonWholeStructureFieldsCollector jsonWholeStructureFieldsCollector, string json, FieldConfig fieldConfig)
        {
            var line = "";
            if(json != "[]")
            {
                var jsonArray = jsonWholeStructureFieldsCollector.GetJsonArray(json);
                line = "[{";
                if (jsonArray.Count > 0)
                {
                    foreach (var fields in jsonArray)
                    {

                        if (line != "[{")
                        {
                            line += ", {";
                        }

                        var fieldCount = 0;

                        foreach (var field in fields.ChildItems)
                        {
                            if (!string.IsNullOrEmpty(field.Contents))
                            {
                                if (fieldCount > 0)
                                {
                                    line += ", ";
                                }
                                var gridFieldConfig = fieldConfig.GridFields.FirstOrDefault(p => p.Id == field.Label);
                                if (gridFieldConfig.Type == "combobox" || gridFieldConfig.Type == "dropdown" || gridFieldConfig.Type == "hierarchicalpicker")
                                {
                                    if (gridFieldConfig.MultiSelect)
                                    {
                                        var multiSelectValue = multiSelectComboProcessor.GetLine(listItemProcessor, field.Contents);
                                        line += string.IsNullOrEmpty(multiSelectValue) ? "" : $"'{field.Label}' : '{multiSelectValue}'";
                                    }
                                    else
                                    {
                                        var listItem = listItemProcessor.GetListItem(field.Contents);
                                        line += string.IsNullOrEmpty(listItem) ? "" : $"'{field.Label}' : '{listItem}'";

                                    }
                                }
                                else
                                {
                                    line += $"'{field.Label}' : '{field.Contents}'";
                                }
                                fieldCount++;
                            }

                        }
                        line += "}";
                    }
                    line += "]";
                }

            }

            return line;
        }
    }
}
