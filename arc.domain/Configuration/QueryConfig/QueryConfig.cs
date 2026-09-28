using arc.common.Data;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig
{
    public class QueryConfig
    {
        public string Query { get; set; }
        public string TableName { get; set; }
        public List<QueryField> Fields { get; set; } = new List<QueryField>();
        public List<QueryJoin> Joins { get; set; }
        public List<QueryWhereFieldConfig> Where { get; set; }
        public List<QueryFilter> Filters { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string RecordReportView { get; set; }
        public string ListItems { get; set; }
        public string MultiSelectItems { get; set; }
        public string OrderBy { get; set; }
        public bool Descending { get; set; }
        public string ParameterMapping { get; set; }
        public string ResultMapping { get; set; }
        public bool Translate { get; set; }
        public string Tags { get; set; }
        public int Limit { get; set; } = 0;

        public string BuildQuery(TokenInfoModel token, QueryFilterConfig filters = null, bool excludeTokenFilters = false)
        {
            var filter = new Filters(filters, Where);

            var jsonConverter = new JsonWholeStructureFieldsCollector();
            var jsonRemover = new JsonElementRemover();
            var moreData = new GenerateMoreData(jsonConverter, jsonRemover);
            var tableNames = new List<string>{TableName};

            if(Joins != null)
            {
                foreach (var join in Joins)
                {
                    tableNames.Add(join.Table);
                }
            }

            var tableFields = moreData.GetFieldLists(tableNames);

            if ((filters == null || filters.Name != "PatientSearch") && ! excludeTokenFilters)
            {
                filter.AddTokenFilter(TableName, token);
            }

            var joinHandler = new Joins(TableName);
            joinHandler.AddJoins(Joins, filters);
            joinHandler.CreateJoinSyntax(ListItems, MultiSelectItems, tableFields.FirstOrDefault(p => p.Key == TableName).Value, filters);

            var joinSql = joinHandler.GetJoinSyntax();
            var fieldListSql = joinHandler.GetFieldSyntax();

            if (Fields != null && Fields.Count > 0)
            {
                foreach (var field in Fields)
                {
                    var knownAs = string.IsNullOrEmpty(field.KnownAs) ? field.Name.Trim() : field.KnownAs;
                    var fieldToDisplay = "s." + field.Name.Trim() + " As " + knownAs;

                    if (field.Type == "specimentags")
                    {
                        fieldToDisplay = "(select string_agg(li.value, ', ') from specimentag st join listitem li on li.id = st.listitemid where st.specimenid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "specimentagids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(st.listitemid::text, ','), '') from specimentag st where st.specimenid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "patienttags")
                    {
                        fieldToDisplay = "(select string_agg(li.value, ', ') from patienttag pt join listitem li on li.id = pt.listitemid where pt.patientid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "patienttagids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(pt.listitemid::text, ','), '') from patienttag pt where pt.patientid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "specimenfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(sfa.fileattachmentid::text, ','), '') from specimenfileattachments sfa where sfa.specimenid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "culturefileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(cfa.fileattachmentid::text, ','), '') from culturefileattachments cfa where cfa.cultureid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "patientfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(pfa.fileattachmentid::text, ','), '') from patientfileattachments pfa where pfa.patientid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "admissionfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(afa.fileattachmentid::text, ','), '') from admissionfileattachments afa where afa.admissionid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "requestfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(rfa.fileattachmentid::text, ','), '') from requestfileattachments rfa where rfa.requestid = s.id) As " + knownAs;
                    }
                    else if (field.Type == "admissionallfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(distinct fid::text, ','), '') from (select afa.fileattachmentid as fid from admissionfileattachments afa where afa.admissionid = s.id union select rfa.fileattachmentid as fid from requestfileattachments rfa inner join request r on r.id = rfa.requestid where r.admissionid = s.id) x) As " + knownAs;
                    }
                    else if (field.Type == "patientallfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(distinct fid::text, ','), '') from (select pfa.fileattachmentid as fid from patientfileattachments pfa where pfa.patientid = s.id union select afa.fileattachmentid as fid from admissionfileattachments afa inner join admission a on a.id = afa.admissionid where a.patientid = s.id union select rfa.fileattachmentid as fid from requestfileattachments rfa inner join request r on r.id = rfa.requestid where r.patientid = s.id) x) As " + knownAs;
                    }
                    else if (field.Type == "instrumentresultfileattachmentids")
                    {
                        fieldToDisplay = "(select coalesce(string_agg(irfa.fileattachmentid::text, ','), '') from instrumentresultfileattachments irfa where irfa.instrumentresultid = s.id) As " + knownAs;
                    }
                    else if (tableFields.FirstOrDefault(p => p.Key == TableName).Value.Count > 0 && !tableFields.FirstOrDefault(p => p.Key == TableName).Value.Contains(field.Name.ToLower()))
                    {
                        var moreDataKey = field.Name.Trim();
                        var moreDataValue = $"COALESCE(s.moredata::jsonb->>'{moreDataKey}', s.moredata::jsonb->>'{moreDataKey.ToLower()}')";

                        if (field.Type == "jdate" || field.Type == "datetime")
                        {
                            fieldToDisplay = $"({moreDataValue})::timestamp";
                        }
                        else
                        {
                            fieldToDisplay = $"{moreDataValue} As {knownAs}";
                        }
                    }

                    if (field.Type == "date")
                    {
                        if (tableFields.FirstOrDefault(p => p.Key == TableName).Value.Count > 0 && !tableFields.FirstOrDefault(p => p.Key == TableName).Value.Contains(field.Name.ToLower()))
                        {
                            var moreDataKey = field.Name.Trim();
                            var moreDataValue = $"COALESCE(s.moredata::jsonb->>'{moreDataKey}', s.moredata::jsonb->>'{moreDataKey.ToLower()}')";
                            fieldToDisplay = $"to_char(cast({moreDataValue} As Date)::DATE, 'yyyy-mm-dd') As {knownAs}";
                        } else
                        {
                            fieldToDisplay = "to_char(" + "s." + field.Name.Trim() + "::DATE, 'yyyy-mm-dd') As " + knownAs;
                        }
                    }

                    if (field.Type == "boolean" && !string.IsNullOrEmpty(field.TrueValue) && !string.IsNullOrEmpty(field.FalseValue))
                    {
                        fieldToDisplay = $"Case When s.{field.Name.Trim()} is TRUE Then '{field.TrueValue}' Else '{field.FalseValue}' End As {knownAs}";
                    }

                    if (field.Type == "jdate" || field.Type == "datetime")
                    {
                        fieldToDisplay = "s." + field.Name.Trim() + " AT TIME ZONE 'UTC' As " + knownAs;
                    }

                    fieldListSql += fieldListSql.Length > 0 ? ", " + fieldToDisplay : fieldToDisplay;
                }
            }

            var whereClause = "";
            if (filter.Parameters != null && filter.Where != null)
            {
                Where = filter.Where;
                whereClause = GetWhereClause(filter.Parameters, fieldListSql, token, tableFields, joinHandler.GetListJoins());
            }

            //Add static filters

            var filterClause = "";
            if (Filters != null)
            {
                foreach (var filterItem in Filters)
                {
                    filterClause = filterClause == "" ? filterItem.GetFilterString() : " and " + filterItem.GetFilterString();
                }
            }

            if (filterClause != "")
            {
                whereClause = whereClause == "" ? "where" + filterClause : whereClause + " and " + filterClause;
            }

            if (Type.ToLower() == "count")
            {
                return "select count(Id) from " + TableName + " s " + joinSql + whereClause + filter.GetSqlOrderString(OrderBy, Descending);
            }

            var strLimit = Limit == 0 ? "" : " Limit " + Limit.ToString();

            return "select distinct " + fieldListSql + " from " + TableName + " s " + joinSql + whereClause + filter.GetSqlOrderString(OrderBy, Descending) + strLimit;
        }

        private string GetWhereClause(List<QueryValuesConfig> parameters, string fieldListSql, TokenInfoModel token, List<KeyValuePair<string, List<string>>> tableFields, List<QueryJoin> listJoins)
        {
            var whereCreator = new CreateWhereClause(token);
            var whereClause = whereCreator.Create(parameters, fieldListSql, Where, TableName, tableFields, Joins, listJoins);

            return whereClause;
        }

        public void AddField(string name, string type)
        {
            var newField = new QueryField { Name = name, Type = type };
            Fields.Add(newField);
        }

        public void AddListItem (string newItem)
        {
            ListItems = string.IsNullOrWhiteSpace(ListItems) ? newItem : ListItems + "," + newItem;
        }

        public void DeleteField(string name)
        {
            Fields.RemoveAll(f => f.Name == name);

            if (name.ToLower().EndsWith("id"))
            {
                name = name.ToLower()[..name.ToLower().LastIndexOf("id")];
            }
            if (ListItems != null)
            {
                var listItemArray = ListItems.Split(",").ToList();
                listItemArray.RemoveAll(v => v.ToLower().Equals(name, System.StringComparison.CurrentCultureIgnoreCase));
                ListItems = string.Join(", ", listItemArray);
            }
       }

    }
}



