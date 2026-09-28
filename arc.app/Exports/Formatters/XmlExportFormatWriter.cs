using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Writes an export as XML by building the same intermediate document tree the JSON writer uses
    /// (via <see cref="IExportDocumentBuilder"/>) and serialising it as XML elements - "creating xml
    /// is just a case of serialising the json".
    /// </summary>
    public class XmlExportFormatWriter : IExportFormatWriter
    {
        private readonly IExportDocumentBuilder _documentBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlExportFormatWriter"/> class.
        /// </summary>
        /// <param name="documentBuilder">Builder that turns rows + mapping into the intermediate tree.</param>
        public XmlExportFormatWriter(IExportDocumentBuilder documentBuilder)
        {
            _documentBuilder = documentBuilder;
        }

        /// <inheritdoc />
        public string Format => "xml";

        /// <inheritdoc />
        public string FileExtension => "xml";

        /// <inheritdoc />
        public string ContentType => "application/xml";

        /// <inheritdoc />
        public string Write(ExportFormatContext context)
        {
            var root = _documentBuilder.Build(context);
            var element = ToElement(root);
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null), element);
            using var writer = new Utf8StringWriter();
            document.Save(writer);
            return writer.ToString();
        }

        /// <summary>
        /// Converts an <see cref="ExportNode"/> to an <see cref="XElement"/>.
        /// </summary>
        private static XElement ToElement(ExportNode node)
        {
            var element = new XElement(SanitiseName(node.Name));
            switch (node.Type)
            {
                case ExportNodeType.Value:
                    element.Value = node.Value ?? string.Empty;
                    break;
                case ExportNodeType.Array:
                case ExportNodeType.Object:
                default:
                    foreach (var child in node.Children)
                    {
                        element.Add(ToElement(child));
                    }
                    break;
            }
            return element;
        }

        /// <summary>
        /// Produces a valid XML element name from a user-supplied mapping name (spaces and invalid
        /// characters are replaced with underscores; names may not start with a digit).
        /// </summary>
        private static string SanitiseName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "value";
            }
            var sb = new StringBuilder();
            foreach (var c in name.Trim())
            {
                sb.Append(char.IsLetterOrDigit(c) || c == '_' || c == '-' ? c : '_');
            }
            var cleaned = sb.ToString();
            if (cleaned.Length == 0 || char.IsDigit(cleaned[0]))
            {
                cleaned = "_" + cleaned;
            }
            return cleaned;
        }

        /// <summary>
        /// String writer that reports UTF-8 encoding so the XML declaration reads utf-8.
        /// </summary>
        private sealed class Utf8StringWriter : System.IO.StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
