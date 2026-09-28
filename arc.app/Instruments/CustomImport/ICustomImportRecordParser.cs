using System.Collections.Generic;
using arc.common.Models.Export;
using arc.common.Models.Instruments.CustomImport;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Parses an inbound Custom-interface file into an <see cref="ImportRecord"/> by walking the export profile mapping
    /// tree in reverse. This is the inverse of the export document builder: attributes are read by their output name and
    /// bound to the export profile field they map to.
    /// </summary>
    public interface ICustomImportRecordParser
    {
        /// <summary>
        /// Parses the supplied file content against the profile mapping and fields.
        /// </summary>
        /// <param name="fileContent">Raw file content (JSON or XML).</param>
        /// <param name="mapping">The export profile mapping (structure + format).</param>
        /// <param name="fields">The export profile fields (used to resolve field keys to field metadata).</param>
        /// <returns>The parsed record hierarchy.</returns>
        ImportRecord Parse(string fileContent, ExportProfileMappingModel mapping, IEnumerable<ExportProfileFieldModel> fields);
    }
}
