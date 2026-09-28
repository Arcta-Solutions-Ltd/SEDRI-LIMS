using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;

namespace arc.app.Configuration.Mappers
{
    /// <summary>
    /// Maps the SectionModel type onto the ReportSectionConfig type.
    /// </summary>
    public class SectionModelToReportSectionConfigMapper : IMapType<SectionModel, ReportSectionConfig>
    {
        /// <summary>
        /// Maps a <see cref="SectionModel"/> object to a <see cref="ReportSectionConfig"/> object.
        /// </summary>
        /// <param name="source">The <see cref="SectionModel"/> object to map.</param>
        /// <returns>The mapped <see cref="ReportSectionConfig"/> object.</returns>
        public ReportSectionConfig Map(SectionModel source)
        {
            var newSection = new ReportSectionConfig
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                HeadingText = source.HeadingText,
                DataSection = source.Source,
                Format = GetFormatName(source.Format)
            };

            newSection.ClearFields();
            var order = 1;
            foreach (var field in source.FieldSelector)
            {
                newSection.AddField(field.Label, field.Value, "", field.Combo, order);
                order++;
            }

            return newSection;
        }

        /// <summary>
        /// Gets the string description for the format from the id
        /// </summary>
        /// <param name="formatNumber">String representing the id of the format</param>
        /// <returns>The description matched from the id</returns>
        private static string GetFormatName(string formatNumber)
        {
            var formatList = new Dictionary<string, string>()
                {
                    { "468", "DoubleColumnOne"},
                    { "469", "DoubleColumnTwo" },
                    { "470", "DoubleColumnThree"},
                    { "471", "DoubleColumnWithGridOne" },
                    { "472", "DoubleColumnWithGridTwo" },
                    { "473", "SingleColumnOne" },
                    { "474", "SingleColumnTwo" },
                    { "475", "TableOne" },
                    { "477", "TableSingleColumn" }
                };
            return formatList[formatNumber];
        }
    }
}


