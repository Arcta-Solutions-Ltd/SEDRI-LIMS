using arc.app.Common;
using arc.app.Configuration;
using arc.common.ExtensionMethods;
using arc.common.Models.Export;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Validates the canonical mapping tree before it is persisted. Mirrors the rules the
    /// front-end editor enforces so any direct API call still produces a safe payload.
    /// </summary>
    public class ExportProfileMappingValidator : IExportProfileMappingValidator
    {
        private static readonly HashSet<string> AllowedFormats = new(StringComparer.OrdinalIgnoreCase) { "json", "xml" };
        private static readonly HashSet<string> AllowedNodeKinds = new(StringComparer.OrdinalIgnoreCase) { "object", "array", "attribute" };
        private static readonly HashSet<string> AllowedArrayTypes = new(StringComparer.OrdinalIgnoreCase) { "specimens", "cultures", "grid", "ast" };

        private readonly IExportProfileMappingFieldOptionBuilder _fieldOptionBuilder;
        private readonly ILogWriter _logWriter;

        // Accumulated during a single ValidateAsync walk. These record intentionally-unmapped
        // nodes (an attribute with no binding, or an array with no type). Such nodes are allowed
        // and will be ignored by the load/unload process; the counts are surfaced for diagnostics.
        private int _unmappedAttributeCount;
        private int _untypedArrayCount;
        private readonly Dictionary<string, int> _uniqueReferenceCounts = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProfileMappingValidator"/> class.
        /// </summary>
        /// <param name="fieldOptionBuilder">Shared builder used to derive the same option set the editor presented to the user.</param>
        /// <param name="logWriter">Log writer for diagnostic information.</param>
        public ExportProfileMappingValidator(
            IExportProfileMappingFieldOptionBuilder fieldOptionBuilder,
            ILogWriter logWriter)
        {
            _fieldOptionBuilder = fieldOptionBuilder;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<ExportProfileMappingValidationResult> ValidateAsync(SaveExportProfileMappingRequest request, IReadOnlyCollection<ExportProfileFieldModel> profileFields, IFormConfigDefinition formConfigDefinition)
        {
            _unmappedAttributeCount = 0;
            _untypedArrayCount = 0;
            _uniqueReferenceCounts.Clear();

            if (request == null)
            {
                Reject("Mapping payload is missing.");
            }

            if (!AllowedFormats.Contains(request.Format ?? string.Empty))
            {
                Reject($"Unsupported mapping format '{request.Format}'.");
            }

            JObject root;
            try
            {
                root = JObject.Parse(string.IsNullOrWhiteSpace(request.Structure) ? "{}" : request.Structure);
            }
            catch (Exception ex)
            {
                Reject($"Mapping structure is not valid JSON: {ex.Message}");
                return new ExportProfileMappingValidationResult();
            }

            // Use the same option set the editor presented (parent field options + one flattened
            // sub-field option per grid column) so any key the user could pick is accepted on save.
            var fieldOptions = await _fieldOptionBuilder.BuildAsync(profileFields, token: null);
            var fieldOptionsByKey = fieldOptions.ToDictionary(o => o.Key, StringComparer.OrdinalIgnoreCase);

            var hasPatient = profileFields.Any(f => string.Equals(f.TableName, "patient", StringComparison.OrdinalIgnoreCase));
            var hasCulture = profileFields.Any(f =>
                string.Equals(f.TableName, "culture", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(f.TableName, "culturetests", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(f.TableName, "ast", StringComparison.OrdinalIgnoreCase));
            var hasAst = profileFields.Any(f => string.Equals(f.TableName, "ast", StringComparison.OrdinalIgnoreCase));

            await ValidateNodeAsync(root, fieldOptionsByKey, profileFields, formConfigDefinition, hasPatient, hasCulture, hasAst, isRoot: true, parentArrayType: null, parentGridFieldKey: null);

            foreach (var entry in _uniqueReferenceCounts)
            {
                if (entry.Value > 1)
                {
                    Reject($"Only one attribute may be marked as the unique reference for {UniqueReferenceTableBucket.DescribeBucket(entry.Key)} fields.");
                }
            }

            if (_unmappedAttributeCount > 0 || _untypedArrayCount > 0)
            {
                _logWriter.LogInfo(
                    $"Mapping validation passed with {_unmappedAttributeCount} unmapped attribute(s) and {_untypedArrayCount} untyped array(s) permitted (ignored by load/unload).",
                    nameof(ExportProfileMappingValidator),
                    nameof(ValidateAsync));
            }

            return new ExportProfileMappingValidationResult
            {
                UnmappedAttributeCount = _unmappedAttributeCount,
                UntypedArrayCount = _untypedArrayCount
            };
        }

        /// <summary>
        /// Recursively validates a node and its children. Attribute nodes with no <c>fieldKey</c> and no
        /// <c>gridSubFieldId</c> are treated as intentionally-unmapped (they are counted and permitted, and
        /// will be ignored by load/unload). Array nodes with no <c>arrayType</c> are treated as untyped and
        /// their whole subtree is permitted-but-ignored, so array-type and nesting constraints are skipped
        /// for them; children are still walked in a neutral (non-array) context.
        /// </summary>
        /// <param name="fieldOptionsByKey">The full option set the editor presented (parent fields plus
        /// flattened grid sub-field options). Used as the allowlist for <c>fieldKey</c> references and to
        /// derive an option's owning table for the AST attribute-table check.</param>
        /// <param name="parentArrayType">The arrayType of the immediate parent when the parent is a typed array, or null otherwise.
        /// Used to enforce that nested arrays are only allowed as AST arrays inside cultures arrays, and to
        /// constrain attribute field references based on context (AST attributes only inside an AST array, etc.).</param>
        /// <param name="parentGridFieldKey">The gridFieldKey of the immediate parent when the parent is a typed
        /// grid array, or null otherwise. Used to resolve the table bucket for grid sub-field attributes.</param>
        private async Task ValidateNodeAsync(
            JObject node,
            IReadOnlyDictionary<string, ExportProfileMappingFieldOption> fieldOptionsByKey,
            IReadOnlyCollection<ExportProfileFieldModel> profileFields,
            IFormConfigDefinition formConfigDefinition,
            bool hasPatient,
            bool hasCulture,
            bool hasAst,
            bool isRoot,
            string parentArrayType,
            string parentGridFieldKey)
        {
            var kind = node.Value<string>("kind");
            if (string.IsNullOrWhiteSpace(kind) || !AllowedNodeKinds.Contains(kind))
            {
                Reject($"Mapping node has invalid kind '{kind}'.");
            }

            switch (kind?.ToLowerInvariant())
            {
                case "object":
                    foreach (var child in EnumerateChildren(node))
                    {
                        await ValidateNodeAsync(child, fieldOptionsByKey, profileFields, formConfigDefinition, hasPatient, hasCulture, hasAst, isRoot: false, parentArrayType: null, parentGridFieldKey: null);
                    }
                    break;

                case "array":
                    var arrayType = node.Value<string>("arrayType");
                    // An array with no type is an intentionally-unmapped (ignored) container: its whole
                    // subtree is skipped by load/unload, so array-type and nesting constraints do not apply.
                    var isUntypedArray = string.IsNullOrWhiteSpace(arrayType);
                    if (isUntypedArray)
                    {
                        _untypedArrayCount++;
                    }
                    else
                    {
                        if (!AllowedArrayTypes.Contains(arrayType))
                        {
                            Reject($"Array node has unsupported arrayType '{arrayType}'.");
                        }

                        if (parentArrayType != null)
                        {
                            var parentIsCultures = parentArrayType.IsSameAs("cultures");
                            var childIsAst = arrayType.IsSameAs("ast");
                            if (!(parentIsCultures && childIsAst))
                            {
                                Reject($"Nested arrays are only allowed as AST arrays inside cultures arrays (got '{arrayType}' inside '{parentArrayType}').");
                            }
                        }

                        if (arrayType.IsSameAs("specimens") && !hasPatient)
                        {
                            Reject("Specimen arrays require at least one patient field on the profile.");
                        }
                        if (arrayType.IsSameAs("cultures") && !hasCulture)
                        {
                            Reject("Culture/isolate arrays require at least one culture, culturetests or ast field on the profile.");
                        }
                        if (arrayType.IsSameAs("ast") && !hasAst)
                        {
                            Reject("AST arrays require at least one AST table field on the profile.");
                        }
                        if (arrayType.IsSameAs("grid"))
                        {
                            await ValidateGridArrayAsync(node, profileFields, formConfigDefinition);
                        }
                    }

                    // Untyped arrays pass a null parent context so their (also-ignored) children are not
                    // held to typed-array attribute rules.
                    var childParentArrayType = isUntypedArray ? null : arrayType;
                    var childParentGridFieldKey = (!isUntypedArray && arrayType.IsSameAs("grid"))
                        ? node.Value<string>("gridFieldKey")
                        : null;
                    foreach (var child in EnumerateChildren(node))
                    {
                        await ValidateNodeAsync(child, fieldOptionsByKey, profileFields, formConfigDefinition, hasPatient, hasCulture, hasAst, isRoot: false, parentArrayType: childParentArrayType, parentGridFieldKey: childParentGridFieldKey);
                    }
                    break;

                case "attribute":
                    if (isRoot)
                    {
                        Reject("Mapping root must be an object, not an attribute.");
                    }
                    var fieldKey = node.Value<string>("fieldKey");
                    var gridSubFieldId = node.Value<string>("gridSubFieldId");
                    var uniqueReference = node.Value<string>("uniqueReference");
                    if (!string.IsNullOrWhiteSpace(uniqueReference))
                    {
                        if (!uniqueReference.IsSameAs("Yes") && !uniqueReference.IsSameAs("No"))
                        {
                            Reject($"Attribute has invalid uniqueReference '{uniqueReference}'.");
                        }
                        if (uniqueReference.IsSameAs("Yes"))
                        {
                            var bucket = UniqueReferenceTableBucket.ResolveAttributeBucket(
                                fieldKey,
                                gridSubFieldId,
                                parentGridFieldKey,
                                fieldOptionsByKey);
                            if (string.IsNullOrWhiteSpace(bucket))
                            {
                                Reject("Unique Reference requires a bound profile field so its table can be determined.");
                            }
                            _uniqueReferenceCounts[bucket] = _uniqueReferenceCounts.TryGetValue(bucket, out var count) ? count + 1 : 1;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(fieldKey) && string.IsNullOrWhiteSpace(gridSubFieldId))
                    {
                        // An attribute with no binding is intentionally unmapped: it describes a field in the
                        // data shape that is not on the profile, so it is permitted and ignored by load/unload.
                        _unmappedAttributeCount++;
                        break;
                    }
                    if (!string.IsNullOrWhiteSpace(fieldKey) && !fieldOptionsByKey.ContainsKey(fieldKey))
                    {
                        Reject($"Attribute references unknown profile field '{fieldKey}'.");
                    }
                    if (!string.IsNullOrWhiteSpace(fieldKey) && fieldOptionsByKey.TryGetValue(fieldKey, out var refOption))
                    {
                        // The referenced option's TableName is shared by parent grid options and any flattened
                        // sub-field options (sub-fields inherit their parent's table), so the AST check here
                        // covers both the legacy path and the flattened-sub-field path.
                        var refIsAst = string.Equals(refOption.TableName, "ast", StringComparison.OrdinalIgnoreCase);
                        var insideAstArray = string.Equals(parentArrayType, "ast", StringComparison.OrdinalIgnoreCase);
                        if (refIsAst && !insideAstArray)
                        {
                            Reject($"Attribute '{fieldKey}' references an AST table field and must be placed inside an AST array.");
                        }
                        if (insideAstArray && !refIsAst)
                        {
                            Reject($"AST array attributes may only reference AST table fields; got '{refOption.TableName}'.");
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Validates that a grid array points at a real grid field on the profile and that
        /// every child attribute references a sub-field defined by that grid.
        /// </summary>
        private async Task ValidateGridArrayAsync(JObject node, IReadOnlyCollection<ExportProfileFieldModel> profileFields, IFormConfigDefinition formConfigDefinition)
        {
            var gridFieldKey = node.Value<string>("gridFieldKey");
            if (string.IsNullOrWhiteSpace(gridFieldKey))
            {
                Reject("Grid arrays must specify the gridFieldKey of the source profile field.");
            }

            var parts = gridFieldKey?.Split('|') ?? Array.Empty<string>();
            if (parts.Length < 2)
            {
                Reject($"Grid arrays have an invalid gridFieldKey '{gridFieldKey}'.");
            }
            var fieldId = parts[0];
            var formName = parts[1];

            var sourceField = profileFields.FirstOrDefault(f =>
                string.Equals(f.FieldName, fieldId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(f.FormName, formName, StringComparison.OrdinalIgnoreCase));
            if (sourceField == null)
            {
                Reject($"Grid array references field '{fieldId}' on form '{formName}' that is not on the profile.");
            }

            var subFieldIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var formConfig = await formConfigDefinition.LoadFormAsync(formName);
                var grid = formConfig.GetGridFieldList(fieldId);
                if (grid == null || grid.Count == 0)
                {
                    Reject($"Field '{fieldId}' on form '{formName}' is not a grid field.");
                }
                foreach (var sub in grid)
                {
                    subFieldIds.Add(sub.Id);
                }
            }
            catch (Exception ex)
            {
                Reject($"Failed to resolve grid definition for {formName}/{fieldId}: {ex.Message}");
            }

            foreach (var child in EnumerateChildren(node))
            {
                if (!string.Equals(child.Value<string>("kind"), "attribute", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                var subId = child.Value<string>("gridSubFieldId");
                if (string.IsNullOrWhiteSpace(subId) || !subFieldIds.Contains(subId))
                {
                    Reject($"Grid array attribute references unknown sub-field '{subId}'.");
                }
            }
        }

        /// <summary>
        /// Returns the JObject children of a node (or empty when none are present).
        /// </summary>
        private static IEnumerable<JObject> EnumerateChildren(JObject node)
        {
            if (node["children"] is JArray array)
            {
                foreach (var child in array.OfType<JObject>())
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Logs and throws an <see cref="InvalidOperationException"/> with the supplied message.
        /// </summary>
        private void Reject(string message)
        {
            _logWriter.LogInfo($"WARN: Mapping validation failed: {message}", nameof(ExportProfileMappingValidator), nameof(ValidateAsync));
            throw new InvalidOperationException(message);
        }
    }
}
