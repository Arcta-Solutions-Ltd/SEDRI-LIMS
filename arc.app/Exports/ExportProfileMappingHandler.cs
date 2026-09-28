using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Handles loading and saving of the JSON or XML structural mapper for an export profile.
    /// Field classification (which fields exist, which are grids, which grid columns are also
    /// available as flattened attributes outside the grid array) is delegated to
    /// <see cref="IExportProfileMappingFieldOptionBuilder"/> so the editor and the validator
    /// agree on the option set.
    /// </summary>
    public class ExportProfileMappingHandler : IExportProfileMappingHandler
    {
        /// <summary>
        /// Default empty structure used when no mapping has been saved against a profile yet.
        /// </summary>
        public const string DefaultStructureJson = "{\"kind\":\"object\",\"name\":\"root\",\"children\":[]}";

        private readonly IExportProfileMappingRepository _repository;
        private readonly IExportProfileFieldRepository _exportProfileFieldRepository;
        private readonly IExportProfileMappingFieldOptionBuilder _fieldOptionBuilder;
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IExportProfileMappingValidator _validator;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProfileMappingHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository used to load and persist mappings.</param>
        /// <param name="exportProfileFieldRepository">Repository used to read the saved profile fields.</param>
        /// <param name="fieldOptionBuilder">Shared builder that produces the field options the editor and validator both use.</param>
        /// <param name="formConfigDefinition">Form definition used by the validator to re-resolve grid sub-fields on save.</param>
        /// <param name="validator">Server-side validator for mapping payloads.</param>
        /// <param name="logWriter">Log writer for diagnostic information.</param>
        public ExportProfileMappingHandler(
            IExportProfileMappingRepository repository,
            IExportProfileFieldRepository exportProfileFieldRepository,
            IExportProfileMappingFieldOptionBuilder fieldOptionBuilder,
            IFormConfigDefinition formConfigDefinition,
            IExportProfileMappingValidator validator,
            ILogWriter logWriter)
        {
            _repository = repository;
            _exportProfileFieldRepository = exportProfileFieldRepository;
            _fieldOptionBuilder = fieldOptionBuilder;
            _formConfigDefinition = formConfigDefinition;
            _validator = validator;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<ExportProfileMappingViewModel> LoadAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            var profileIdParam = queryFilters?.Parameters?.FirstOrDefault(p =>
                string.Equals(p.Key, "ExportProfileId", StringComparison.OrdinalIgnoreCase))
                ?? queryFilters?.Parameters?.FirstOrDefault(p =>
                    string.Equals(p.Key, "id", StringComparison.OrdinalIgnoreCase));
            if (profileIdParam == null || !int.TryParse(profileIdParam.Value, out var profileId))
            {
                _logWriter.LogInfo("WARN: ExportProfileMappingHandler.LoadAsync called without a valid ExportProfileId", nameof(ExportProfileMappingHandler), nameof(LoadAsync));
                return new ExportProfileMappingViewModel { Structure = DefaultStructureJson };
            }

            _logWriter.LogInfo($"Loading mapping for export profile {profileId}", nameof(ExportProfileMappingHandler), nameof(LoadAsync));

            // Make sure the downstream queries always have an ExportProfileId parameter even when the
            // form launcher sent only a generic "id" parameter (mirrors ExportProfileFieldsForProfileIdQuery).
            var profileFilters = EnsureProfileIdParameter(queryFilters, profileId);

            var existing = await _repository.GetByProfileIdAsync(profileFilters);
            var fields = (await _exportProfileFieldRepository.GetByProfileIdAsync(profileFilters)).ToList();

            var fieldOptions = await _fieldOptionBuilder.BuildAsync(fields, token);

            var hasPatient = fields.Any(f => string.Equals(f.TableName, "patient", StringComparison.OrdinalIgnoreCase));
            var hasCulture = fields.Any(f =>
                string.Equals(f.TableName, "culture", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(f.TableName, "culturetests", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(f.TableName, "ast", StringComparison.OrdinalIgnoreCase));
            var gridFieldCount = fieldOptions.Count(o => o.IsGridField);
            var flattenedSubFieldCount = fieldOptions.Count(o => !string.IsNullOrEmpty(o.ParentGridKey));
            var astFieldCount = fields.Count(f => string.Equals(f.TableName, "ast", StringComparison.OrdinalIgnoreCase));

            _logWriter.LogInfo(
                $"Mapping load: profileId={profileId}, fieldCount={fields.Count}, hasPatient={hasPatient}, hasCulture={hasCulture}, gridFieldCount={gridFieldCount}, flattenedSubFieldCount={flattenedSubFieldCount}, astFieldCount={astFieldCount}, hasExisting={(existing != null)}",
                nameof(ExportProfileMappingHandler),
                nameof(LoadAsync));

            return new ExportProfileMappingViewModel
            {
                ExportProfileId = profileId,
                Format = string.IsNullOrWhiteSpace(existing?.Format) ? "json" : existing.Format,
                Structure = string.IsNullOrWhiteSpace(existing?.Structure) ? DefaultStructureJson : existing.Structure,
                HasPatientFields = hasPatient,
                HasCultureFields = hasCulture,
                FieldOptions = fieldOptions
            };
        }

        /// <inheritdoc />
        public async Task<int> SaveAsync(SaveExportProfileMappingRequest request)
        {
            if (request == null)
            {
                _logWriter.LogInfo("WARN: ExportProfileMappingHandler.SaveAsync called with null request", nameof(ExportProfileMappingHandler), nameof(SaveAsync));
                throw new ArgumentNullException(nameof(request));
            }

            _logWriter.LogInfo(
                $"Saving mapping for profile {request.ExportProfileId}, format={request.Format}",
                nameof(ExportProfileMappingHandler),
                nameof(SaveAsync));

            var fields = (await _exportProfileFieldRepository.GetByProfileIdAsync(
                new QueryFilterConfig().AddInteger("ExportProfileId", request.ExportProfileId))).ToList();
            var validationResult = await _validator.ValidateAsync(request, fields, _formConfigDefinition);

            if (validationResult != null && (validationResult.UnmappedAttributeCount > 0 || validationResult.UntypedArrayCount > 0))
            {
                _logWriter.LogInfo(
                    $"Saving mapping for profile {request.ExportProfileId} with {validationResult.UnmappedAttributeCount} unmapped attribute(s) and {validationResult.UntypedArrayCount} untyped array(s); these nodes are ignored by load/unload.",
                    nameof(ExportProfileMappingHandler),
                    nameof(SaveAsync));
            }

            var model = new ExportProfileMappingModel
            {
                ExportProfileId = request.ExportProfileId,
                Format = request.Format?.ToLowerInvariant() ?? "json",
                Structure = string.IsNullOrWhiteSpace(request.Structure) ? DefaultStructureJson : request.Structure
            };
            var serialized = JsonConvert.SerializeObject(model);
            var savedId = await _repository.SaveAsync(serialized);

            _logWriter.LogInfo(
                $"Mapping saved: id={savedId}, profileId={request.ExportProfileId}, format={model.Format}",
                nameof(ExportProfileMappingHandler),
                nameof(SaveAsync));

            return savedId;
        }

        /// <summary>
        /// Builds a copy of <paramref name="source"/> guaranteed to expose an "ExportProfileId" parameter.
        /// The form launcher may send only a generic "id" parameter when the page is opened from a list
        /// or record view; downstream queries are written against the explicit "ExportProfileId" key.
        /// </summary>
        /// <param name="source">The original query filters (may be null).</param>
        /// <param name="profileId">The resolved profile id.</param>
        /// <returns>A query filter that contains an ExportProfileId parameter.</returns>
        private static QueryFilterConfig EnsureProfileIdParameter(QueryFilterConfig source, int profileId)
        {
            var filters = new QueryFilterConfig();
            if (source?.Parameters != null)
            {
                foreach (var p in source.Parameters)
                {
                    filters.Parameters.Add(p);
                }
            }
            if (!filters.Parameters.Any(p => string.Equals(p.Key, "ExportProfileId", StringComparison.OrdinalIgnoreCase)))
            {
                filters.AddInteger("ExportProfileId", profileId);
            }
            return filters;
        }
    }
}
