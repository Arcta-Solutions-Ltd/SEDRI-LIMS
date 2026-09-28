using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Writes an export as JSON by building the intermediate document tree (via
    /// <see cref="IExportDocumentBuilder"/>) and serialising it.
    /// </summary>
    public class JsonExportFormatWriter : IExportFormatWriter
    {
        private readonly IExportDocumentBuilder _documentBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonExportFormatWriter"/> class.
        /// </summary>
        /// <param name="documentBuilder">Builder that turns rows + mapping into the intermediate tree.</param>
        public JsonExportFormatWriter(IExportDocumentBuilder documentBuilder)
        {
            _documentBuilder = documentBuilder;
        }

        /// <inheritdoc />
        public string Format => "json";

        /// <inheritdoc />
        public string FileExtension => "json";

        /// <inheritdoc />
        public string ContentType => "application/json";

        /// <inheritdoc />
        public string Write(ExportFormatContext context)
        {
            var root = _documentBuilder.Build(context);
            var token = ToToken(root);
            return token.ToString(Formatting.Indented);
        }

        /// <summary>
        /// Converts an <see cref="ExportNode"/> to its JSON representation.
        /// </summary>
        private static JToken ToToken(ExportNode node)
        {
            switch (node.Type)
            {
                case ExportNodeType.Array:
                    var array = new JArray();
                    foreach (var child in node.Children)
                    {
                        array.Add(ToToken(child));
                    }
                    return array;
                case ExportNodeType.Value:
                    return new JValue(node.Value ?? string.Empty);
                case ExportNodeType.Object:
                default:
                    var obj = new JObject();
                    foreach (var child in node.Children)
                    {
                        obj[child.Name] = ToToken(child);
                    }
                    return obj;
            }
        }
    }
}
