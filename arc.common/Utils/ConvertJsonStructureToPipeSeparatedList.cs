using arc.common.Models.Reports;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils
{
    /// <summary>
    /// Flattens the grid arrays held in a saved test's JSON into pipe separated rows, one group per grid.
    /// </summary>
    public class ConvertJsonStructureToPipeSeparatedList : IConvertJsonStructureToPipeSeparatedList
    {
        private readonly IJsonWholeStructureFieldsCollector _jsonWholeStructureFieldsCollector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertJsonStructureToPipeSeparatedList"/> class.
        /// </summary>
        /// <param name="jsonWholeStructureFieldsCollector">Collector that walks the saved JSON into a flat structure of labelled items.</param>
        public ConvertJsonStructureToPipeSeparatedList(IJsonWholeStructureFieldsCollector jsonWholeStructureFieldsCollector)
        {
            _jsonWholeStructureFieldsCollector = jsonWholeStructureFieldsCollector;
        }

        /// <summary>
        /// Converts every array in the saved JSON into a group of pipe separated rows.
        /// </summary>
        /// <param name="json">The test's saved JSON.</param>
        /// <returns>
        /// One entry per array found, in the order the arrays appear. Each entry's Label is the JSON
        /// property name, which is the form fieldgrid id the report matches its grid bindings against,
        /// and its Contents are the array's rows with the cell values joined by a pipe.
        /// </returns>
        public List<GridLineContentsModel> Convert(string json)
        {
            var jsonStructure = _jsonWholeStructureFieldsCollector.GetStructure(json);

            var arrayList = jsonStructure.Where((item) => item.ArrayItems != null);

            var contents = new List<GridLineContentsModel>();
            var lines = new List<string>();
            var currentItem = "";
            foreach (var item in arrayList)
            {
                if (currentItem != item.Label)
                {
                    if (lines.Count > 0)
                    {
                        contents.Add(new GridLineContentsModel { Label = currentItem, Contents = lines });
                        lines = new List<string>();
                    }
                    currentItem = item.Label;
                }
                foreach(var el in item.ArrayItems)
                {
                    string line = "";
                    foreach (var e in el.ChildItems)
                    {
                        line += line == "" ? e.Contents.Trim() : "|" + e.Contents.Trim();
                    }
                    lines.Add(line);
                }
            }

            if (lines.Count > 0)
            {
                contents.Add(new GridLineContentsModel { Label = currentItem, Contents = lines });
            }

            return contents;
        }

    }
}
