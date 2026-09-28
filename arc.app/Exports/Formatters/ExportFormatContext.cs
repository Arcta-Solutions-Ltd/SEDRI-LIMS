using System.Collections.Generic;
using arc.common.Models.Export;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Immutable-ish input passed to an <see cref="IExportFormatWriter"/>. Carries the fully
    /// post-processed pipe-delimited rows together with the metadata a format writer needs to
    /// re-shape them (the aligned column keys and, for JSON/XML, the profile's mapping tree).
    /// </summary>
    public class ExportFormatContext
    {
        /// <summary>
        /// Gets or sets the post-processed rows. Index 0 is the header row; the remainder are data
        /// rows. Each row is a pipe-delimited (<c>|</c>) set of column values.
        /// </summary>
        public List<string> Lines { get; set; } = new();

        /// <summary>
        /// Gets or sets the column keys aligned one-to-one with the columns of every row in
        /// <see cref="Lines"/>. Each key identifies the source field by id (never by translated header)
        /// so the JSON/XML writers can resolve mapping attributes without ambiguity. Hidden grouping-id
        /// columns carry sentinel keys (see <see cref="ExportColumnKeys"/>).
        /// </summary>
        public List<string> ColumnKeys { get; set; } = new();

        /// <summary>
        /// Gets or sets the ordered export profile fields (used for diagnostics and future formatters).
        /// </summary>
        public List<ExportProfileFieldModel> Fields { get; set; } = new();

        /// <summary>
        /// Gets or sets the profile's structural mapping, or null when the profile has no mapping.
        /// </summary>
        public ExportProfileMappingModel Mapping { get; set; }

        /// <summary>
        /// Gets or sets the export profile id (used for diagnostics only).
        /// </summary>
        public string ProfileId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Sentinel column keys used for the hidden id columns appended to each row so that the
    /// JSON/XML writers can group rows into nested arrays by stable ids rather than by display values.
    /// </summary>
    public static class ExportColumnKeys
    {
        /// <summary>Sentinel key for the hidden specimen id column.</summary>
        public const string SpecimenId = "__specimenid";

        /// <summary>Sentinel key for the hidden culture id column.</summary>
        public const string CultureId = "__cultureid";

        /// <summary>Sentinel key for the hidden ast id column.</summary>
        public const string AstId = "__astid";
    }
}
