using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// <see cref="IImportDocument"/> over an XML element (<see cref="XElement"/>). Mirrors the export XML writer: arrays
    /// are elements whose children are repeated <c>item</c> elements, and attributes are child elements holding text.
    /// Element name lookups are case-insensitive.
    /// </summary>
    public class XmlImportDocument : IImportDocument
    {
        private readonly XElement _element;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlImportDocument"/> class.
        /// </summary>
        /// <param name="element">The XML element this document wraps.</param>
        public XmlImportDocument(XElement element)
        {
            _element = element ?? new XElement("root");
        }

        private XElement Find(string name) =>
            _element.Elements()
                .FirstOrDefault(e => string.Equals(e.Name.LocalName, name, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc />
        public string GetAttributeValue(string name)
        {
            var child = Find(name);
            if (child == null || child.HasElements)
            {
                return null;
            }
            return child.Value;
        }

        /// <inheritdoc />
        public IEnumerable<IImportDocument> GetArrayItems(string arrayName)
        {
            var container = Find(arrayName);
            if (container == null)
            {
                return Enumerable.Empty<IImportDocument>();
            }
            var items = container.Elements()
                .Where(e => string.Equals(e.Name.LocalName, "item", StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (items.Count == 0)
            {
                // Lenient: treat direct child elements as items when no explicit <item> wrappers exist.
                items = container.Elements().ToList();
            }
            return items.Select(e => (IImportDocument)new XmlImportDocument(e)).ToList();
        }

        /// <inheritdoc />
        public IImportDocument GetChildObject(string name)
        {
            var child = Find(name);
            return child != null && child.HasElements ? new XmlImportDocument(child) : null;
        }
    }
}
