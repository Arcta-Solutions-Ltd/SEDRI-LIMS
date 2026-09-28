namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// The clinical record level an <see cref="ImportEntity"/> or <see cref="CustomImportEntity"/> represents.
    /// Levels mirror the export profile buckets used by the mapping tree.
    /// </summary>
    public enum ImportEntityType
    {
        /// <summary>Patient level (export bucket <c>patient</c>).</summary>
        Patient,

        /// <summary>Specimen level (export bucket <c>specimen</c>).</summary>
        Specimen,

        /// <summary>Direct test on a specimen (export bucket <c>tests</c>).</summary>
        DirectTest,

        /// <summary>Culture / isolate level (export bucket <c>culture</c>).</summary>
        Culture,

        /// <summary>Isolate test on a culture (export bucket <c>culturetests</c>).</summary>
        CultureTest,

        /// <summary>Antibiotic susceptibility result on a culture (export bucket <c>ast</c>).</summary>
        Ast
    }
}
