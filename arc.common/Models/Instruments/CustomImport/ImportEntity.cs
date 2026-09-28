using System.Collections.Generic;
using System.Linq;

namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// One parsed entity in the inbound record hierarchy (patient / specimen / direct test / culture / isolate test / AST).
    /// Values are held as raw <see cref="ImportFieldValue"/> instances; list/tag values are resolved to ids later by the
    /// import handler so all matching is language independent.
    /// </summary>
    public class ImportEntity
    {
        /// <summary>The record level this entity represents.</summary>
        public ImportEntityType EntityType { get; set; }

        /// <summary>Field values extracted for this entity.</summary>
        public List<ImportFieldValue> Fields { get; set; } = new();

        /// <summary>Nested entities (e.g. a specimen's direct tests and cultures; a culture's isolate tests and AST rows).</summary>
        public List<ImportEntity> Children { get; set; } = new();

        /// <summary>Returns the child entities of the supplied type.</summary>
        public IEnumerable<ImportEntity> ChildrenOfType(ImportEntityType type) =>
            Children.Where(c => c.EntityType == type);
    }
}
