using System;
using System.Collections.Generic;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// The kind of value an import field holds. Drives how a value is resolved before matching/saving.
    /// </summary>
    public enum ImportFieldKind
    {
        /// <summary>Free text stored as-is.</summary>
        String,

        /// <summary>Numeric value.</summary>
        Number,

        /// <summary>List/combobox value that must be resolved to a list item id (language independent).</summary>
        List,

        /// <summary>Date value.</summary>
        Date
    }

    /// <summary>
    /// Metadata describing how a well-known export profile field maps to a database column for the Custom import.
    /// </summary>
    public class ImportFieldMetadata
    {
        /// <summary>Database column the field maps to.</summary>
        public string Column { get; set; }

        /// <summary>The value kind (drives id resolution).</summary>
        public ImportFieldKind Kind { get; set; }

        /// <summary>List name used to resolve <see cref="ImportFieldKind.List"/> values to ids.</summary>
        public string ListName { get; set; }
    }

    /// <summary>
    /// Static registry mapping well-known export profile field names (per source table) to database columns for the
    /// Custom-interface import. Fields not present here are stored in the entity's <c>moredata</c> JSON. List columns are
    /// resolved to ids so matching is language independent (see business rule "match by id, not value").
    /// </summary>
    public static class CustomImportFieldRegistry
    {
        private static readonly Dictionary<string, Dictionary<string, ImportFieldMetadata>> Registry =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["patient"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["PatientRef"] = new() { Column = "patientref", Kind = ImportFieldKind.String },
                    ["FirstName"] = new() { Column = "firstname", Kind = ImportFieldKind.String },
                    ["Surname"] = new() { Column = "surname", Kind = ImportFieldKind.String },
                    ["Gender"] = new() { Column = "genderid", Kind = ImportFieldKind.List, ListName = "gender" },
                    ["DateOfBirth"] = new() { Column = "dateofbirth", Kind = ImportFieldKind.Date },
                    ["Age"] = new() { Column = "age", Kind = ImportFieldKind.String },
                    ["TelephoneNumber"] = new() { Column = "telephonenumber", Kind = ImportFieldKind.String },
                    ["AddressLine1"] = new() { Column = "addressline1", Kind = ImportFieldKind.String },
                    ["AddressLine2"] = new() { Column = "addressline2", Kind = ImportFieldKind.String },
                    ["ZipCode"] = new() { Column = "zipcode", Kind = ImportFieldKind.String },
                    ["Barcode"] = new() { Column = "barcode", Kind = ImportFieldKind.String }
                },
                ["specimen"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["AccessionNumber"] = new() { Column = "accessionnumber", Kind = ImportFieldKind.String },
                    ["Barcode"] = new() { Column = "barcode", Kind = ImportFieldKind.String },
                    ["ExistingBarcode"] = new() { Column = "existingbarcode", Kind = ImportFieldKind.String },
                    ["SpecimenType"] = new() { Column = "specimentypeid", Kind = ImportFieldKind.List, ListName = "SpecimenType" },
                    ["CollectionDate"] = new() { Column = "collectiondate", Kind = ImportFieldKind.Date },
                    ["ReceivedDate"] = new() { Column = "receiveddate", Kind = ImportFieldKind.Date },
                    ["ClinicalContactNo"] = new() { Column = "clinicalcontactno", Kind = ImportFieldKind.String }
                },
                ["culture"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["CultureNumber"] = new() { Column = "culturenumber", Kind = ImportFieldKind.Number },
                    ["CultureType"] = new() { Column = "typeid", Kind = ImportFieldKind.List, ListName = "CultureType" },
                    ["Growth"] = new() { Column = "growthid", Kind = ImportFieldKind.List, ListName = "SpecimenGrowth" },
                    ["SpecimenOrganismId"] = new() { Column = "specimenorganismid", Kind = ImportFieldKind.Number },
                    ["AdditionalNotes"] = new() { Column = "additionalnotes", Kind = ImportFieldKind.String },
                    ["AloquatId"] = new() { Column = "aloquatid", Kind = ImportFieldKind.String }
                },
                ["ast"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["AntibioticId"] = new() { Column = "antibioticid", Kind = ImportFieldKind.Number },
                    ["Susceptibility"] = new() { Column = "susceptibilityid", Kind = ImportFieldKind.List, ListName = "susceptibility" },
                    ["Category"] = new() { Column = "categoryid", Kind = ImportFieldKind.List, ListName = "category" },
                    ["TestMethod"] = new() { Column = "testmethodid", Kind = ImportFieldKind.List, ListName = "testmethod" },
                    ["Measurement"] = new() { Column = "measurement", Kind = ImportFieldKind.Number },
                    ["TestType"] = new() { Column = "testtype", Kind = ImportFieldKind.String },
                    ["EntryType"] = new() { Column = "entrytype", Kind = ImportFieldKind.String }
                }
            };

        /// <summary>
        /// Returns the column metadata for a field within the given bucket, or null when the field has no dedicated column
        /// (in which case it should be stored in the entity's <c>moredata</c>).
        /// </summary>
        /// <param name="bucket">Normalized bucket (patient / specimen / culture / ast).</param>
        /// <param name="fieldName">Source field name (case-insensitive).</param>
        public static ImportFieldMetadata Resolve(string bucket, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fieldName))
            {
                return null;
            }

            return Registry.TryGetValue(bucket.Trim(), out var fields)
                   && fields.TryGetValue(fieldName.Trim(), out var metadata)
                ? metadata
                : null;
        }
    }
}
