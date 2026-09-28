using arc.common.Models.Config;
using arc.common.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace arc.common.Data
{
    /// <summary>
    /// Contains methods for working out what should be contained in the MoreData field within a table.
    /// </summary>
    public class GenerateMoreData : IGenerateMoreData
    {
        private IJsonWholeStructureFieldsCollector _jsonConverter;
        private readonly IJsonElementRemover _jsonRemover;
        private JObject _leftOverData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateMoreData"/> class.
        /// </summary>
        /// <param name="jsonConverter">Collector used to flatten the whole json structure of a save message.</param>
        /// <param name="jsonRemover">Remover used to strip client metadata elements from a save message.</param>
        public GenerateMoreData(IJsonWholeStructureFieldsCollector jsonConverter, IJsonElementRemover jsonRemover)
        {
            _jsonConverter = jsonConverter;
            _jsonRemover = jsonRemover;
            _leftOverData = new JObject();
        }

        /// <summary>
        /// Returns the json string that should be stored in the Moredata field for a table.
        /// </summary>
        /// <param name="table">Name of the table containing the moredata string</param>
        /// <param name="message">Json message containing all the data to be saved to the table from which the moredata string will be created</param>
        /// <param name="tableExceptions">Contains list of values that should not be included in the moredata field. This is in addition to the values that would be stored in a different field within the table under consideration</param>
        /// <returns>Json string containing the data to be stored in the moredata field of the table under consideration</returns>
        public string GetMoreDataJsonString(string table, string message, List<TableExceptionModel> tableExceptions = null)
        {

            foreach (var metadataField in ClientPayloadMetadataFields.Names)
            {
                message = _jsonRemover.RemoveElementsByValue(message, metadataField);
            }

            var jsonFields = _jsonConverter.GetStructure(message);
            var tableFields = GetFieldList(table);
            var exclusionList = GetExclusionList(table);
            var jsonObject = new JObject();
            _leftOverData = new JObject();

            tableFields.AddRange(exclusionList);

            if (tableExceptions != null && tableExceptions.Count() > 0)
            {
                List<string> exceptionList = tableExceptions.Select(e => e.Field.ToLower()).ToList();
                tableFields.AddRange(exceptionList);
            }

            if (tableFields.Count() > 0)
            {
                foreach (var field in jsonFields)
                {
                    if (field.ArrayItems == null)
                    {
                        if (!tableFields.Contains(field.Label.ToLower()))
                        {
                            if (field.Contents != null && field.Label.ToLower() != "event" && !field.Label.StartsWith("metaf"))
                            {
                                jsonObject.Add(field.Label, field.Contents);
                            }
                        }
                        else
                        {
                            _leftOverData.Add(field.Label, field.Contents);
                        }
                    }
                }
            }

            return jsonObject.ToString();
        }

        /// <summary>
        /// Returns the json string that contains the values from a json string matching the keys in the table exceptions list.
        /// </summary>
        /// <param name="table">Name of the table containing the moredata string.</param>
        /// <param name="message">Json message containing all the data from which the values will be extracted.</param>
        /// <param name="tableExceptions">Contains list of values that need to be extracted from the message. Only those matching the currently selected table will be returned.</param>
        /// <returns>Json string containing the data extracted from the message for the specified table</returns>
        public string GetExceptionListString(string table, string message, List<TableExceptionModel> tableExceptions = null)
        {
            var jsonFields = _jsonConverter.GetStructure(message);
            var jsonObject = new JObject();

            if (tableExceptions != null)
            {
                List<string> tableFields = tableExceptions.Where(e => e.Table.ToLower() == table.ToLower()).Select(e => e.Field.ToLower()).ToList();

                if (tableFields.Count > 0)
                {
                    foreach (var field in jsonFields)
                    {
                        if (field.ArrayItems == null)
                        {
                            if (tableFields.Contains(field.Label.ToLower()))
                            {
                                if (field.Contents != null && field.Label.ToLower() != "event")
                                {
                                    jsonObject.Add(field.Label, field.Contents);
                                }
                            }
                        }
                    }
                }
            }

            return jsonObject.ToString();
        }

        /// <summary>
        /// Returns the json string containing the fields from the last processed message that map to real
        /// columns on the table, and so belong in the insert or update statement rather than in MoreData.
        /// </summary>
        /// <returns>Json string of the fields left over after the MoreData field was built.</returns>
        public string GetLeftOverData()
        {
            return _leftOverData.ToString();
        }

        /// <summary>
        /// Returns the names of the fields that are stored in their own column on the specified table.
        /// Any other field in a save message for that table is a candidate for the MoreData field.
        /// </summary>
        /// <param name="tableName">Name of the table under consideration.</param>
        /// <returns>Lower case column names for the table, or an empty list when the table has no MoreData field.</returns>
        public List<string> GetFieldList(string tableName)
        {
            switch (tableName.ToLower())
            {
                case "admission":
                    return new List<string> { "id", "patientid", "lastmodifieddate" };
                case "culture":
                    return new List<string> { "culturenumber", "specimenorganismid", "specimenquantityid", "specimenapiidpanelid", "idprofile", "idpercentage", "positivedate", "positivetime",
                                              "commentoneid", "commenttwoid", "additionalnotes", "astadditionalnotes", "aloquatid", "displayonreport", "id", "growthid",
                                              "specimenid", "typeid", "orggroupcodingid", "alerttypeid", "lastmodifieddate", "astcommentoneid", "astcommenttwoid", "parentcultureid" };
                case "instrumentresults":
                    return new List<string> { "id", "instrumentprofile", "specimenid", "cultureid", "barcode", "requestmade", "lastmodifieddate", "resultreceived", "statusid", "moredata", "instrumentmachineid", "accessionnumber", "culturenumber" };
                case "location":
                    return new List<string> { "enabled", "name", "fullyqualifiedname", "parentlocationid", "code", "id", "lastmodifieddate", "longitude", "latitude" };
                case "organisation":
                    return new List<string> { "enabled", "organisationname", "fullyqualifiedname", "parentorganisationid", "languageid", "id", "lastmodifieddate", "locationid", "code" };
                case "patient":
                    return new List<string> { "patientref", "surname", "firstname", "age", "dateofbirth", "id", "lastmodifieddate", "telephonenumber", "stateid", "genderid",
                                              "locationid", "addressline1", "addressline2", "zipcode", "barcode"};
                case "queue":
                    return new List<string> { "id", "recordid", "message", "username", "added", "topicid", "eventid", "eventstatusid", "specimenid", "patientid", "stateid", "error", "hash" };
                case "request":
                    return new List<string> { "id", "patientid", "admissionid", "requestid", "lastmodifieddate" };
                case "specimen":
                    return new List<string> { "collectiondate","collectiontime","receiveddate","receivedtime","specimentypeid","specimensiteid",
                        "patientlocationid","organisationid","laboratoryid","admissiondate","diagnosisid","clinicalcontactno","specimenweight","receivedconditionid",
                        "specimenappearanceid","existingbarcode", "id", "view", "barcode", "stateid", "actionid", "patientid", "lastmodifieddate", "accessionnumber",
                        "bottleonlyweight","bloodandbottleweight", "alerttypeid", "rejectionreason", "reasonone", "reasontwo", "approvalcommentl1id", "approvalcommentl2id",
                        "ageyears", "agemonths", "agedays", "agehours", "admissionid", "requestid" };
                case "storage":
                    return ["id", "storagename", "fullyqualifiedname", "description", "storagetypeid", "temperature", "code", "parentstorageid", "lastmodifieddate", "enabled"];
                case "users":
                    return new List<string> { "enabled", "firstname", "lastname", "password", "username", "email", "organisationid", "id" };
                case "exportprofilerecord":
                    return new List<string> {   "ordernumber", "modifieddate", "labelname", "formname", "headername", "fieldname", "tablename", "exportprofileid", "id" };
                case "specimentag":
                    return new List<string> { "id", "specimenid", "listitemid", "lastmodifieddate" };
                case "exportrunhistory":
                    return new List<string> { "id", "exportprofileid", "filter", "runat", "fileattachmentid" };
                case "patienttag":
                    return new List<string> { "id", "patientid", "listitemid", "lastmodifieddate" };
                case "listitem":
                    return new List<string> { "id", "listid", "value", "fixed", "enabled", "deleted", "parentid", "lastmodifieddate", "displayorder" };
                default:
                    return new List<string>();
            }
        }

        /// <summary>
        /// Returns the column name list for each of the specified tables.
        /// </summary>
        /// <param name="tableNames">Names of the tables under consideration.</param>
        /// <returns>One entry per table, pairing the table name with its column names.</returns>
        public List<KeyValuePair<string, List<string>>> GetFieldLists(List<string> tableNames)
        {
            var list = new List<KeyValuePair<string, List<string>>>();
            foreach (var tableName in tableNames)
            {
                list.Add(new KeyValuePair<string, List<string>>(tableName, GetFieldList(tableName)));
            }
            return list;
        }

        /// <summary>
        /// Returns names of fields that must never be written to the MoreData field of the specified table,
        /// even though they are not columns on that table. These are values that belong to a related record.
        /// </summary>
        /// <param name="tableName">Name of the table under consideration.</param>
        /// <returns>Lower case field names to exclude from MoreData, or an empty list when there are none.</returns>
        private List<string> GetExclusionList(string tableName)
        {
            switch (tableName.ToLower())
            {
                case "specimen":
                    return new List<string> { "surname", "genderid","firstname", "districtid", "provinceid","subdistrictid", "dateofbirthasdate","admissiondateasdate", "surnamesearch", "locationsearch",
                        "collectiondateasdate", "addressline1", "addressline2", "zipcode", "age", "action", "gender", "values", "locationid", "dateofbirth", "telephonenumber", "patientrefsearch" };
                default:
                    return new List<string>();
            }
        }
    }
}

