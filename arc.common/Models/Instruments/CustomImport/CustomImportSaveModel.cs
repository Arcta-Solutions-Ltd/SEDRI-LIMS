using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// A resolved entity ready to be upserted by <c>ICustomImportRepository.SaveRecordAsync</c>. All list/tag values in
    /// <see cref="Columns"/> have already been resolved to ids so matching and saving are language independent.
    /// </summary>
    public class CustomImportEntity
    {
        /// <summary>The record level this entity represents.</summary>
        public ImportEntityType EntityType { get; set; }

        /// <summary>Database column used to match an existing row (e.g. <c>patientref</c>, <c>accessionnumber</c>, <c>culturenumber</c>, <c>testname</c>).</summary>
        public string ReferenceColumn { get; set; }

        /// <summary>Value of <see cref="ReferenceColumn"/> for this entity (used to find an existing row).</summary>
        public string ReferenceValue { get; set; }

        /// <summary>Resolved column values (ids already resolved for list/tag columns).</summary>
        public Dictionary<string, object> Columns { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Values that have no dedicated column and are stored in the entity's <c>moredata</c> JSON.</summary>
        public Dictionary<string, string> MoreData { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Test/isolate-test result values (field name -&gt; value) serialized into <c>testresults</c> JSON.</summary>
        public Dictionary<string, string> TestResults { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Nested resolved entities.</summary>
        public List<CustomImportEntity> Children { get; set; } = new();

        /// <summary>Returns the resolved child entities of the supplied type.</summary>
        public IEnumerable<CustomImportEntity> ChildrenOfType(ImportEntityType type) =>
            Children.Where(c => c.EntityType == type);
    }

    /// <summary>
    /// The complete resolved save request for one inbound Custom-interface record. Passed to the repository so the whole
    /// record is saved inside a single transaction.
    /// </summary>
    public class CustomImportSaveModel
    {
        /// <summary>Instrument profile name the record was loaded for (used for error reporting).</summary>
        public string ProfileName { get; set; }

        /// <summary>True when the profile mapping has patient-bucket fields; when false a new specimen must attach to an existing patient.</summary>
        public bool CreatePatients { get; set; }

        /// <summary>Resolved patient entity (null when <see cref="CreatePatients"/> is false).</summary>
        public CustomImportEntity Patient { get; set; }

        /// <summary>Resolved specimen entities (each may contain direct tests and cultures).</summary>
        public List<CustomImportEntity> Specimens { get; set; } = new();

        /// <summary>Laboratory id used when inserting new specimens (from the authenticated token).</summary>
        public int LaboratoryId { get; set; }

        /// <summary>Organisation id used when inserting new specimens (from the authenticated token).</summary>
        public int OrganisationId { get; set; }
    }
}
