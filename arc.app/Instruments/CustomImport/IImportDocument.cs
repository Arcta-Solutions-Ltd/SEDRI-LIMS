using System.Collections.Generic;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Format-agnostic view over one node (object) of an inbound Custom-interface document. JSON and XML implementations
    /// let the parser walk the export mapping tree without caring about the underlying format.
    /// </summary>
    public interface IImportDocument
    {
        /// <summary>Returns the text value of the named attribute/element, or null when absent.</summary>
        string GetAttributeValue(string name);

        /// <summary>Returns the object items of the named array (JSON array items or XML repeated <c>item</c> elements).</summary>
        IEnumerable<IImportDocument> GetArrayItems(string arrayName);

        /// <summary>Returns the named child object node, or null when absent.</summary>
        IImportDocument GetChildObject(string name);
    }
}
