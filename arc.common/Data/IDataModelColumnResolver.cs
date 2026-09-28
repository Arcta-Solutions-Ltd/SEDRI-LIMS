namespace arc.common.Data;

/// <summary>
/// Determines whether a form field id maps to a physical database column or to MoreData,
/// using <c>arc.data.model</c> classes resolved via <see cref="arc.data.model.DataModelUtils"/>.
/// </summary>
public interface IDataModelColumnResolver
{
    /// <summary>
    /// Returns true when <paramref name="fieldId"/> matches a public property on the table's
    /// data model (excluding the MoreData JSONB sink property).
    /// </summary>
    /// <param name="tableName">Database table name (e.g. patient, specimen).</param>
    /// <param name="fieldId">Form field identifier from page config.</param>
    /// <returns>True when the field maps to a physical column.</returns>
    bool IsPhysicalColumn(string tableName, string fieldId);

    /// <summary>
    /// Returns true when the field should be stored in the table's MoreData JSONB blob.
    /// </summary>
    /// <param name="tableName">Database table name.</param>
    /// <param name="fieldId">Form field identifier from page config.</param>
    /// <returns>True when the field is not a physical column on the target table.</returns>
    bool IsMoreDataField(string tableName, string fieldId);
}
