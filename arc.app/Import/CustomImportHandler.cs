using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.Instruments.CustomImport;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Instruments;

namespace arc.app.Import
{
    /// <summary>
    /// Default <see cref="ICustomImportHandler"/>. Resolves list/tag values to ids, splits direct/isolate test result
    /// fields (grouped by their form name) from entity columns, and delegates the transactional save to the repository.
    /// </summary>
    public class CustomImportHandler : ICustomImportHandler
    {
        private readonly IImportValueResolver _valueResolver;
        private readonly ICustomImportRepository _repository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImportHandler"/> class.
        /// </summary>
        /// <param name="valueResolver">Resolver used to convert list/tag display values to ids.</param>
        /// <param name="repository">Repository that persists the record transactionally.</param>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public CustomImportHandler(IImportValueResolver valueResolver, ICustomImportRepository repository, ILogWriter logWriter)
        {
            _valueResolver = valueResolver;
            _repository = repository;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<CustomImportSaveResult> ImportAsync(ImportRecord record, UniqueReferenceMap references, SingleInstrumentConfig profile, TokenInfoModel token)
        {
            var model = new CustomImportSaveModel
            {
                ProfileName = profile?.InstrumentName,
                CreatePatients = record.HasPatientData,
                LaboratoryId = token?.LaboratoryId.ToInt() ?? 0,
                OrganisationId = token?.OrganisationId.ToInt() ?? 0
            };

            if (record.Patient != null)
            {
                model.Patient = await BuildEntityAsync(record.Patient, ImportEntityType.Patient, "patient", "patient", references.PatientField, "patientref");
            }

            foreach (var specimen in record.Specimens)
            {
                model.Specimens.Add(await BuildSpecimenAsync(specimen, references));
            }

            _logWriter.LogInfo(
                $"Custom import: saving record for profile '{model.ProfileName}' (createPatients={model.CreatePatients}, specimens={model.Specimens.Count})",
                nameof(CustomImportHandler), nameof(ImportAsync));

            return await _repository.SaveRecordAsync(model);
        }

        private async Task<CustomImportEntity> BuildSpecimenAsync(ImportEntity src, UniqueReferenceMap references)
        {
            var entity = await BuildEntityAsync(src, ImportEntityType.Specimen, "specimen", "specimen", references.SpecimenField, "accessionnumber");

            foreach (var test in BuildTests(src.Fields, "tests", ImportEntityType.DirectTest))
            {
                entity.Children.Add(test);
            }

            foreach (var culture in src.ChildrenOfType(ImportEntityType.Culture))
            {
                entity.Children.Add(await BuildCultureAsync(culture, references));
            }

            return entity;
        }

        private async Task<CustomImportEntity> BuildCultureAsync(ImportEntity src, UniqueReferenceMap references)
        {
            var entity = await BuildEntityAsync(src, ImportEntityType.Culture, "culture", "culture", references.CultureField, "culturenumber");

            foreach (var test in BuildTests(src.Fields, "culturetests", ImportEntityType.CultureTest))
            {
                entity.Children.Add(test);
            }

            foreach (var ast in src.ChildrenOfType(ImportEntityType.Ast))
            {
                entity.Children.Add(await BuildEntityAsync(ast, ImportEntityType.Ast, "ast", "ast", null, "antibioticid"));
            }

            return entity;
        }

        private async Task<CustomImportEntity> BuildEntityAsync(ImportEntity src, ImportEntityType type, string bucket, string ownTable, string referenceFieldName, string defaultReferenceColumn)
        {
            var entity = new CustomImportEntity { EntityType = type };

            foreach (var field in src.Fields.Where(f => string.Equals(f.TableName?.Trim(), ownTable, StringComparison.OrdinalIgnoreCase)))
            {
                var resolved = await _valueResolver.ResolveAsync(bucket, field.FieldName, field.Value);
                var metadata = CustomImportFieldRegistry.Resolve(bucket, field.FieldName);
                if (metadata != null)
                {
                    entity.Columns[metadata.Column] = resolved;
                }
                else if (!string.IsNullOrWhiteSpace(field.Value))
                {
                    entity.MoreData[field.FieldName] = field.Value;
                }
            }

            var referenceColumn = defaultReferenceColumn;
            if (!string.IsNullOrWhiteSpace(referenceFieldName))
            {
                var referenceMetadata = CustomImportFieldRegistry.Resolve(bucket, referenceFieldName);
                if (referenceMetadata != null)
                {
                    referenceColumn = referenceMetadata.Column;
                }
            }

            entity.ReferenceColumn = referenceColumn;
            entity.ReferenceValue = entity.Columns.TryGetValue(referenceColumn, out var refValue) ? refValue?.ToString() : null;
            return entity;
        }

        private static IEnumerable<CustomImportEntity> BuildTests(List<ImportFieldValue> fields, string tableName, ImportEntityType type)
        {
            var testFields = fields.Where(f => string.Equals(f.TableName?.Trim(), tableName, StringComparison.OrdinalIgnoreCase));
            foreach (var group in testFields.GroupBy(f => f.FormName ?? string.Empty))
            {
                if (string.IsNullOrWhiteSpace(group.Key))
                {
                    continue;
                }
                var test = new CustomImportEntity
                {
                    EntityType = type,
                    ReferenceColumn = "testname",
                    ReferenceValue = group.Key
                };
                foreach (var field in group)
                {
                    if (!string.IsNullOrWhiteSpace(field.Value))
                    {
                        test.TestResults[field.FieldName] = field.Value;
                    }
                }
                yield return test;
            }
        }
    }
}
