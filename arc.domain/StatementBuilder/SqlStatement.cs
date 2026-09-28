using arc.common.Data;
using arc.common.Utils;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.StatementBuilder
{
    public class SqlStatement(string mainTable)
    {
        internal List<SqlField> Fields = [];
        internal List<SqlWhere> Where = [];
        internal List<SqlJoin> Joins = new List<SqlJoin>();
        private readonly string _mainTable = mainTable;
        private readonly PrefixHandler _prefixHandler = new();
        public string FieldString { get; set; }
        public string JoinString { get; set; }
        public bool Distinct { get; set; } = false;

        public void AddField(SqlField sqlField)
        {
            Fields.Add(sqlField);
        }

        private void AddJoin(string prefix, string table, string linkPrefix, string linkField, string linkToTable, List<string> joinClauses, string prefixField, bool isCustom, string testField = "", bool isTest = false, string listPrefix = "")
        {
            var joinAlreadyExists = table == "listitem" || table == "testres" || table == "culturetestres" || table == "specmoredata" || table == "patientmoredata" || (!Joins.Any(j => j.Table.ToLower() == table.ToLower()));

            if (joinAlreadyExists)
            {
                var newJoin = new SqlJoin { Prefix = prefix, Table = table, LinkPrefix = linkPrefix, LinkField = linkField, LinkTable = linkToTable, TestField = testField, IsTest = isTest, ListPrefix = listPrefix, JoinClauses = joinClauses,
                    PrefixField = prefixField, IsCustom = isCustom};
                Joins.Add(newJoin);
            }
        }

        public string CreateSql()
        {

            var mainPrefix = _prefixHandler.GetPrefix(_mainTable);
            FieldString = Distinct ? "distinct concat(" : "concat(";

            var jsonConverter = new JsonWholeStructureFieldsCollector();
            var jsonRemover = new JsonElementRemover();
            var moreData = new GenerateMoreData(jsonConverter, jsonRemover);

            var fieldCount = 0;

            // Create select string
            foreach (var field in Fields)
            {
                var table = field.TableName.ToLower();

                if(fieldCount  % 45 == 0 && fieldCount > 0)
                {
                    FieldString += ",'|') || concat(";
                }
                else 
                {
                    FieldString += FieldString == "concat(" || FieldString == "distinct concat(" ? "" : ",'|',";

                }

                var tableFields = moreData.GetFieldList(table);

                if (table == "tests" || table == "culturetests" || field.JoinTable.ToLower() == "reporthistory")
                {

                    UpdateTestFieldSyntax(field, field.JoinTable.ToLower() == "reporthistory" ? "reporthistory" : table);
                } 
                else
                {
                    if (field.AlternateIfNull)
                    {
                        FieldString += AlternateFieldSyntax(field);
                    } else
                    {
                        if(field.InlcudeAdditionalJoin)
                        {
                            FieldString += DoubleJoinFieldSyntax(field);
                        }
                        else
                        {
                            if (tableFields.Count > 0 && !tableFields.Contains(field.FieldName.ToLower()) && field.FormName != "Custom")
                            {
                                UpdateTestFieldSyntax(field, table);
                            }
                            else
                            {
                                if(field.FieldName == "BlankColumn")
                                {
                                    FieldString += "''";
                                }
                                else
                                {
                                    if (field.FieldName == "Measurement" && field.IncludeMICComparison == "Yes")
                                    {
                                        FieldString += "ast.miccomparison," + SingleFieldSyntax(field.TableName, field.FieldName, field.JoinTable, field.JoinField, field.JoinClauses, field.PrefixField, field.FormName == "Custom");
                                    }
                                    else
                                    {
                                        if(field.FieldConfigId == "AccessionIsolateNumber")
                                        {
                                            var iso = SingleFieldSyntax(field.TableName, field.FieldName, field.JoinTable, field.JoinField, field.JoinClauses, field.PrefixField, field.FormName == "Custom");
                                            FieldString += $"Case when {iso} is null then cast({iso} as varchar) else concat(s.accessionnumber,{iso}) end";
                                        }
                                        else
                                        {
                                            FieldString += SingleFieldSyntax(field.TableName, field.FieldName, field.JoinTable, field.JoinField, field.JoinClauses, field.PrefixField, field.FormName == "Custom");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                fieldCount++;
            }

            FieldString += ")";

            // Create join string

            JoinString = "";
            foreach (var join in Joins)
            {
                if (join.IsTest)
                {
                    JoinString += " " + join.GetTestJoinSyntax();
                }
                else
                {
                    JoinString += " " + join.GetJoinSyntax();
                }
            }

            var sql = "select " + FieldString + " from " + _mainTable + " " + mainPrefix + JoinString;
            return sql;
        }

        public void SetInitialPrefixesToUse(string table, string prefix)
        {
            _prefixHandler.SetInitialPrefix(table, prefix);
        }

        private string FieldSyntax(string prefix, string fieldName)
        {
            var returnString =  prefix + "." + fieldName;

            return returnString;
        }

        public string AlternateFieldSyntax(SqlField field)
        {
            var mainFieldSyntax = SingleFieldSyntax(field.TableName, field.FieldName, field.JoinTable, field.JoinField, field.JoinClauses, field.PrefixField, field.FormName == "Custom");

            var alternateFieldSyntax = "";
            if (field.UseAdditionalForAlternative)
            {
                alternateFieldSyntax = DoubleJoinFieldSyntax(field);
            }
            else
            {
                 alternateFieldSyntax = SingleFieldSyntax(field.TableName, field.AlternateFieldName, field.AlternateJoinTable, field.AlternateJoinField, field.AlternativeJoinClauses, field.AlternatePrefixField, field.FormName == "Custom");
            };

            return "Case when " + mainFieldSyntax + " is null then " + alternateFieldSyntax + " else " + mainFieldSyntax + " end";
        }

        private string SingleFieldSyntax(string tableName, string fieldName, string joinTable, string joinField, List<string> joinClauses, string prefixField, bool isCustom)
        {
            var prefix = _prefixHandler.GetPrefix(tableName);

            var returnSyntax = "";

            if (!string.IsNullOrEmpty(joinTable))
            {
                var joinPrefix = _prefixHandler.GetPrefix(joinTable);
                returnSyntax += joinPrefix + "." + joinField.Trim().ToLower();
                AddJoin(joinPrefix, joinTable, prefix, fieldName, tableName, joinClauses, prefixField, isCustom);
            }
            else
            {
                returnSyntax = FieldSyntax(prefix, fieldName);
            }

            return returnSyntax;
        }

        private string DoubleJoinFieldSyntax(SqlField field)
        {
            var firstPrefix = _prefixHandler.GetPrefix(field.TableName);
            var firstJoinPrefix = _prefixHandler.GetPrefix(field.JoinTable);

            if (field.UseAdditionalForAlternative)
            {
                firstPrefix = _prefixHandler.GetPrefix(field.TableName);
                firstJoinPrefix = _prefixHandler.GetPrefix(field.AlternateJoinTable);
                AddJoin(firstJoinPrefix, field.AlternateJoinTable, firstPrefix, field.AlternateFieldName, field.TableName, field.AlternativeJoinClauses, field.AlternatePrefixField, field.FormName == "Custom");
            }
            else
            {
                AddJoin(firstJoinPrefix, field.JoinTable, firstPrefix, field.FieldName, field.TableName, field.JoinClauses, field.PrefixField, field.FormName == "Custom");
            }

            var secondJoinPrefix = _prefixHandler.GetPrefix(field.AdditionalJoinTable);
            AddJoin(secondJoinPrefix, field.AdditionalJoinTable, firstJoinPrefix, field.AdditionalFieldName, field.AdditionalTableName, field.AdditionalJoinClauses, field.AdditionalPrefixField, field.FormName == "Custom");
            return secondJoinPrefix + "." + field.AdditionalJoinField.Trim().ToLower();
        }

        private void UpdateTestFieldSyntax(SqlField field, string testtype = "tests")
        {
            var prefix = "";
            var linkField = "";
            var joinTable = "";
            var isTest = true;
            var value = ".value";

            switch (testtype)
            {
                case "tests":
                    prefix = "s";
                    linkField = "specimenid";
                    joinTable = "testres";
                    break;
                case "culturetests":
                    prefix = "c";
                    linkField = "cultureid";
                    joinTable = "culturetestres";
                    break;
                case "specimen":
                    prefix = "s";
                    linkField = "id";
                    joinTable = "specmoredata";
                    break;
                case "patient":
                    prefix = "p";
                    linkField = "id";
                    joinTable = "patientmoredata";
                    break;
                case "reporthistory":
                    prefix = "s";
                    linkField = "specimenid";
                    joinTable = "reportdateres";
                    isTest = false;
                    value = ".max";
                    break;
            }

            var joinPrefix = _prefixHandler.GetPrefix("testres");

            var listPrefix = "";
            if (field.Type == "list" && !field.MultiSelect)
            {
                listPrefix = _prefixHandler.GetPrefix("listitem");
                FieldString += listPrefix + ".value";
            } else
            {
                FieldString += joinPrefix + value;
            }

            AddJoin(joinPrefix, joinTable, prefix, field.PrefixField, joinTable, field.JoinClauses, linkField, field.FormName == "Custom", field.FieldName, isTest, listPrefix);
        }
    }
}



