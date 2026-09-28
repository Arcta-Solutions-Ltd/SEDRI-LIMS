using System;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Identifies database tables whose events are patient-scoped and must not run specimen workflow resolution.
/// </summary>
public static class EventWorkflowTableNames
{
    /// <summary>
    /// Returns true when the table name is a patient-scoped entity (patient, patientcomment, patienttag).
    /// </summary>
    /// <param name="tableName">Event configuration table name.</param>
    /// <returns>True when the table is patient-scoped.</returns>
    public static bool IsPatientScopedTable(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            return false;
        }

        return tableName.Trim().Equals("patient", StringComparison.OrdinalIgnoreCase)
            || tableName.Trim().Equals("patientcomment", StringComparison.OrdinalIgnoreCase)
            || tableName.Trim().Equals("patienttag", StringComparison.OrdinalIgnoreCase)
            || tableName.Trim().Equals("admission", StringComparison.OrdinalIgnoreCase)
            || tableName.Trim().Equals("request", StringComparison.OrdinalIgnoreCase);
    }
}
