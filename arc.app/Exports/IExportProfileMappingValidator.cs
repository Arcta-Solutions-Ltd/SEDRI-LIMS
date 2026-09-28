using arc.app.Configuration;
using arc.common.Models.Export;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Server-side validator for an export profile mapping payload. Enforces the same
    /// constraints the Manage Mapping editor presents on the client (allowed formats,
    /// attribute leaves limited to profile fields, conditional array types).
    /// </summary>
    public interface IExportProfileMappingValidator
    {
        /// <summary>
        /// Validates a save request against the supplied profile fields. Throws
        /// <see cref="System.InvalidOperationException"/> when any constraint is violated.
        /// Attributes with no field binding and arrays with no type are permitted (they are
        /// ignored by the load/unload process); the returned result reports how many such
        /// unmapped nodes were accepted.
        /// </summary>
        /// <param name="request">The save request submitted by the editor.</param>
        /// <param name="profileFields">The fields currently saved on the profile.</param>
        /// <param name="formConfigDefinition">Used to re-derive grid sub-fields for grid arrays.</param>
        /// <returns>A result describing how many unmapped attributes / untyped arrays were permitted.</returns>
        Task<ExportProfileMappingValidationResult> ValidateAsync(SaveExportProfileMappingRequest request, IReadOnlyCollection<ExportProfileFieldModel> profileFields, IFormConfigDefinition formConfigDefinition);
    }
}
