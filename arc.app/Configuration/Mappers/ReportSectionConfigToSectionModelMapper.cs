using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;

namespace arc.app.Configuration.Mappers
{
    /// <summary>
    /// Maps the ReportSectionConfig type onto the SectionModel type.
    /// </summary>
    public class ReportSectionConfigToSectionModelMapper : IMapType<ReportSectionConfig, SectionModel>
    {
        /// <summary>
        /// Maps a <see cref="ReportSectionConfig"/> object to a <see cref="SectionModel"/> object.
        /// </summary>
        /// <param name="source">The <see cref="ReportSectionConfig"/> object to map.</param>
        /// <returns>The mapped <see cref="SectionModel"/> object.</returns>
        public SectionModel Map(ReportSectionConfig source)
        {
            var newSection = new SectionModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                HeadingText = source.HeadingText,
                Source = source.DataSection,
                Format = GetFormatId(source.Format),
                FieldSelector = new List<SectionFieldSelectorModel>()
            };

            foreach (var field in source.Fields)
            {
                var newItem = new SectionFieldSelectorModel { Label = field.Label, Value = field.Value, Combo = field.Column };
                newSection.FieldSelector.Add(newItem);
            }

            return newSection;
        }

        /// <summary>
        /// Gets the id of a string entry from the string description
        /// </summary>
        /// <param name="format">The string description</param>
        /// <returns>The id matched against the string description</returns>
        private static string GetFormatId(string format)
        {
            var formatList = new Dictionary<string, string>()
                {
                    { "Double column (1)", "468"},
                    { "Double column (2)", "469" },
                    { "Double column (3)", "470"},
                    { "Double column with grid (1)", "471" },
                    { "Double column with grid (2)", "472" },
                    { "Single column (1)", "473" },
                    { "Single column (2)", "474" },
                    { "Single column with dynamic header", "476" },
                    { "Table", "475" },
                    { "DoubleColumnOne", "468"},
                    { "DoubleColumnTwo", "469" },
                    { "DoubleColumnThree", "470"},
                    { "DoubleColumnWithGridOne", "471" },
                    { "DoubleColumnWithGridTwo", "472" },
                    { "SingleColumnOne", "473" },
                    { "SingleColumnTwo", "474" },
                    { "TableOne", "475" },
                    { "TableSingleColumn", "477" },
                    { "DynamicSingleColumnOne", "476" }
                };
            return formatList[format];
        }
    }
}


