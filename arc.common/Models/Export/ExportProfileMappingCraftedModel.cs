using System.Collections.Generic;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Crafted-page wrapper model used to deserialize the Manage Mapping save payload.
    /// Mirrors the shape produced by the standard form save pipeline for crafted pages
    /// (a single Crafted entry whose Contents holds one keyed value).
    /// </summary>
    public class ExportProfileMappingCraftedModel
    {
        /// <summary>
        /// Gets or sets the crafted page name (e.g. manageexportprofilemappingpage).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the keyed contents of the crafted page (one entry for the mapping payload).
        /// </summary>
        public List<ExportProfileMappingCraftedKeyValueModel> Contents { get; set; }
    }

    /// <summary>
    /// Single keyed entry inside <see cref="ExportProfileMappingCraftedModel.Contents"/>.
    /// The key is always "mapping"; the value carries the full save request.
    /// </summary>
    public class ExportProfileMappingCraftedKeyValueModel
    {
        /// <summary>
        /// Gets or sets the change key (always "mapping").
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the save request payload.
        /// </summary>
        public SaveExportProfileMappingRequest Value { get; set; }
    }
}
