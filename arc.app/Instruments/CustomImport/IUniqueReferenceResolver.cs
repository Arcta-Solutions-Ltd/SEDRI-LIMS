using System.Collections.Generic;
using arc.common.Models.Export;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// The unique-reference field name used to match existing records for each bucket. Values are export profile field
    /// names (e.g. <c>PatientRef</c>, <c>AccessionNumber</c>, <c>CultureNumber</c>).
    /// </summary>
    public class UniqueReferenceMap
    {
        /// <summary>Field name that uniquely identifies a patient. Defaults to <c>PatientRef</c>.</summary>
        public string PatientField { get; set; } = "PatientRef";

        /// <summary>Field name that uniquely identifies a specimen. Defaults to <c>AccessionNumber</c>.</summary>
        public string SpecimenField { get; set; } = "AccessionNumber";

        /// <summary>Field name that uniquely identifies a culture within a specimen. Defaults to <c>CultureNumber</c>.</summary>
        public string CultureField { get; set; } = "CultureNumber";
    }

    /// <summary>
    /// Resolves the unique-reference field per bucket from the export profile mapping. When an attribute is marked as the
    /// unique reference (<c>uniqueReference: "Yes"</c>) for its bucket, that field is used; otherwise the bucket default
    /// applies (patient: PatientRef, specimen: AccessionNumber, culture: CultureNumber).
    /// </summary>
    public interface IUniqueReferenceResolver
    {
        /// <summary>
        /// Builds the <see cref="UniqueReferenceMap"/> from the parsed mapping structure and the export profile fields.
        /// </summary>
        /// <param name="structureJson">The mapping <c>structure</c> JSON.</param>
        /// <param name="fields">The export profile fields (used to resolve field keys to field names).</param>
        UniqueReferenceMap Resolve(string structureJson, IEnumerable<ExportProfileFieldModel> fields);
    }
}
