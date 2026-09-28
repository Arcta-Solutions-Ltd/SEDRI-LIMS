using arc.common.Utils;
using arc.domain.Configuration.PagesConfig;
using arc.domain.StatementBuilder;
using System.Collections.Generic;
using System.Linq;

namespace arc.data.Utils
{
    internal class ExportFieldBuilder
    {
        private string _sqlString = "";
        private string _joins = "";

        internal string BuildSql(List<FieldConfig> fieldConfigs, IJsonReplacer jsonReplacer)
        {

            // Work out whether the report should be at the patient culture or specimen level

            var containsCultureFields = fieldConfigs.Any(r => r.TableName.ToLower() == "culture" || r.TableName.ToLower() == "culturetests");
            var containsSpecimenFields = fieldConfigs.Any(r => r.TableName.ToLower() == "specimen" || r.TableName.ToLower() == "tests");
            var containsPatientFields = fieldConfigs.Any(r => r.TableName.ToLower() == "patient");
            var mainTable = containsCultureFields ? "culture" : containsSpecimenFields ? "specimen" : "patient";

            var sqlStatement = new SqlStatement(mainTable);
            sqlStatement.SetInitialPrefixesToUse("specimen", "s");
            sqlStatement.SetInitialPrefixesToUse("culture", "c");
            sqlStatement.SetInitialPrefixesToUse("patient", "p");
            sqlStatement.SetInitialPrefixesToUse("ast", "ast");

            sqlStatement.Distinct = mainTable == "patient";

            // Work out whether the report should be at the patient culture or specimen level

            foreach (var fieldConfig in fieldConfigs)
            {
                var newField = new SqlField
                {
                    FieldName = fieldConfig.Id.Trim(),
                    FieldConfigId = fieldConfig.Id.Trim(),
                    JsonField = fieldConfig.TableName.ToLower() == "tests" || fieldConfig.TableName.ToLower() == "culturetests" ? "testresults" : "",
                    MultiSelect = fieldConfig.MultiSelect,
                    TableName = fieldConfig.TableName,
                    Type = fieldConfig.Type,
                    FormName = fieldConfig.Label
                };

                if (newField.Type == "list" && newField.JsonField == "")
                {
                    newField.FieldName = newField.FieldName.ToLower().EndsWith("id") ? newField.FieldName : newField.FieldName + "id";
                }

                if (newField.Type == "list" && !newField.MultiSelect)
                {
                    newField.JoinField = "value";
                    newField.JoinTable = "listitem";
                }

                if (newField.Type == "list" && newField.MultiSelect)
                {
                    newField.JoinField = "value";
                }

                if(fieldConfig.IsComment)
                {
                    newField.JoinTable = "specimencomment";
                    newField.JoinField = "comment";
                    newField.FieldName = "id";
                    newField.JoinClauses = new List<string>(["fieldid = '" + fieldConfig.Id.Trim().ToLower() + "'"]);
                    newField.PrefixField = fieldConfig.TableName.ToLower() == "specimen" ? "specimenid" : "cultureid";

                    if (fieldConfig.Type == "list")
                    {
                        newField.InlcudeAdditionalJoin = true;
                        newField.JoinField = "cannedcomment";
                        newField.AdditionalFieldName = "cannedcommentid";
                        newField.AdditionalJoinField = "value";
                        newField.AdditionalJoinTable = "listitem";
                        newField.AdditionalTableName = "specimencomment";
                    }
                }

                switch (newField.FieldName.ToLower()) 
                {
                    case "fullyqualifiedname":
                        newField.JoinTable = "location";
                        newField.JoinField = "fullyqualifiedname";
                        newField.FieldName = "locationid";
                        newField.TableName = "patient";
                        break;
                    case "locationid":
                        newField.JoinTable = "location";
                        newField.JoinField = "name";
                        newField.FieldName = "locationid";
                        newField.TableName = "patient";
                        break;
                    case "fullorganisationname":
                        newField.JoinTable = "organisation";
                        newField.JoinField = "fullyqualifiedname";
                        newField.FieldName = "organisationid";
                        newField.TableName = "specimen";
                        break;
                    case "organisationid":
                        newField.JoinTable = "organisation";
                        newField.JoinField = "organisationname";
                        newField.FieldName = "organisationid";
                        newField.TableName = "specimen";
                        break;
                    case "locationcodehierarchy":
                        newField.JoinTable = "location";
                        newField.JoinField = "moredata ->> 'LocationCodeHierarchy'";
                        newField.FieldName = "locationid";
                        newField.TableName = "patient";
                        break;
                    case "organisationcodehierarchy":
                        newField.JoinTable = "organisation";
                        newField.JoinField = "moredata ->> 'OrganisationCodeHierarchy'";
                        newField.FieldName = "organisationid";
                        newField.TableName = "specimen";
                        break;
                    case "laboratoryname":
                        newField.JoinTable = "laboratory";
                        newField.FieldName = "laboratoryid";
                        newField.JoinField = "laboratoryname";
                        newField.TableName = "specimen";
                        break;
                    case "specimenorganism":
                        newField.JoinTable = "organisms";
                        newField.JoinField = "OrganismName";
                        newField.FieldName = "specimenorganismid";
                        break;
                    case "organismnameorgrowth":
                        newField.JoinTable = "organisms";
                        newField.JoinField = "OrganismName";
                        newField.FieldName = "specimenorganismid";
                        
                        newField.AlternateIfNull = true;
                        newField.AlternateJoinTable = "listitemparentchild";
                        newField.AlternateJoinField = "parentid";
                        newField.AlternateFieldName = "growthid";
                        newField.AlternatePrefixField = "childid";

                        newField.UseAdditionalForAlternative = true;
                        newField.AdditionalTableName = "listitemparentchild";
                        newField.AdditionalJoinTable = "listitem";
                        newField.AdditionalJoinField = "value";
                        newField.AdditionalFieldName = "parentid";
                        break;
                    case "organismpreferredname":
                        newField.JoinTable = "organismsynonyms";
                        newField.JoinField = "synonym";
                        newField.FieldName = "specimenorganismid";
                        newField.PrefixField = "organismid";

                        newField.AlternateIfNull = true;
                        newField.AlternateJoinTable = "organisms";
                        newField.AlternateJoinField = "organismname";
                        newField.AlternateFieldName = "specimenorganismid";
                        newField.JoinClauses = new List<string>(["preferredname = true"]);
                        break;
                    case "approvaldate":
                        newField.JoinTable = "specimenstatehistory";
                        newField.JoinField = "lastmodifieddate";
                        newField.FieldName = "id";
                        newField.PrefixField = "specimenid";
                        newField.JoinClauses = new List<string>(["stateid = 534"]);
                        break;
                    case "specimenstate":
                        newField.JoinTable = "listitem";
                        newField.JoinField = "value";
                        newField.FieldName = "stateid";
                        newField.PrefixField = "id";
                        break;  
                    case "reportdate":
                        newField.JoinTable = "reporthistory";
                        newField.JoinField = "lastmodifieddate";
                        newField.FieldName = "id";
                        newField.PrefixField = "id";
                        break;

                    case "antibioticid":
                        newField.JoinTable = "antibiotic";
                        newField.JoinField = "antibioticname";
                        newField.FieldName = "antibioticid";
                        newField.PrefixField = "id";
                        break;
                    case "guidelinesid":
                        newField.JoinTable = "listitem";
                        newField.JoinField = "value";
                        newField.FieldName = "guidelinesid";
                        newField.PrefixField = "id";
                        break;
                    case "susceptibilityid":
                        newField.JoinTable = "listitem";
                        newField.JoinField = "value";
                        newField.FieldName = "susceptibilityid";
                        newField.PrefixField = "id";
                        break;
                    case "categoryid":
                        newField.JoinTable = "listitem";
                        newField.JoinField = "value";
                        newField.FieldName = "categoryid";
                        newField.PrefixField = "id";
                        break;
                    case "testmethodid":
                        newField.JoinTable = "listitem";
                        newField.JoinField = "value";
                        newField.FieldName = "testmethodid";
                        newField.PrefixField = "id";
                        break;
                    case "measurement":
                        newField.IncludeMICComparison = !string.IsNullOrEmpty(fieldConfig.MoreData) && fieldConfig.MoreData != "{}" ? jsonReplacer.GetValueInJsonString(fieldConfig.MoreData, "IncludeMICComparison") : "No";
                        break;
                    case "accessionisolatenumber":
                        newField.FieldName = "culturenumber";
                        break;
                }

                sqlStatement.AddField(newField);
            }

            var statement = sqlStatement.CreateSql();
            _sqlString = sqlStatement.FieldString;
            _joins = sqlStatement.JoinString;
            return statement;
        }

        internal string GetJoins()
        {
             return _joins;
        }

        internal string GetSqlString()
        {
            return _sqlString;
        }
    }
}
