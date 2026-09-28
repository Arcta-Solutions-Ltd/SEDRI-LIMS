using System.Collections.Generic;

namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// The single logical record parsed from one inbound Custom-interface file: an optional patient plus its specimens
    /// (each with direct tests and cultures; each culture with isolate tests and AST rows). This is the reverse of the
    /// document produced by the export mapping tree.
    /// </summary>
    public class ImportRecord
    {
        /// <summary>Patient entity when the profile mapping contains patient-bucket fields; otherwise null.</summary>
        public ImportEntity Patient { get; set; }

        /// <summary>Specimen entities parsed from the document (specimens array, or the record root when flat).</summary>
        public List<ImportEntity> Specimens { get; set; } = new();

        /// <summary>True when the profile mapping contained at least one patient-bucket field.</summary>
        public bool HasPatientData { get; set; }
    }
}
