using arc.app.Configuration;
using arc.common.Models;
using arc.common.Models.Export;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Builds the classified <see cref="ExportProfileMappingFieldOption"/> set the Manage Mapping editor
    /// (and the server-side validator) treats as the authoritative list of selectable attribute targets.
    /// Each profile field becomes one option; grid (direct-test / culture-test) fields additionally
    /// contribute one flattened option per sub-column so the column can be placed outside its grid array.
    /// </summary>
    public interface IExportProfileMappingFieldOptionBuilder
    {
        /// <summary>
        /// Builds the option list for a profile.
        /// </summary>
        /// <param name="fields">The export profile records.</param>
        /// <param name="token">Optional token used for label translation. May be null (the keys
        /// produced are independent of translation, so the validator can pass null).</param>
        /// <returns>The classified field options.</returns>
        Task<List<ExportProfileMappingFieldOption>> BuildAsync(
            IReadOnlyCollection<ExportProfileFieldModel> fields,
            TokenInfoModel token);
    }
}
