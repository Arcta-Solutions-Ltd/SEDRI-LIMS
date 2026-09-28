using arc.common.Data;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.StatementBuilder;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig
{
    public class Joins
    {
        private List<QueryJoin> _joins;
        private string _joinSql;
        private string _fieldSql;
        private string _alias;
        private string _tableName;
        private List<QueryJoin> _listJoins = [];
        private readonly PrefixHandler _prefixHandler = new();

        public Joins(string tableName = "")
        {
            _joins = new List<QueryJoin>();
            _joinSql = "";
            _fieldSql = "";
            _alias = "a";
            _tableName = tableName;
        }

        internal void AddJoin(QueryJoin joinToAdd)
        {
            if (joinToAdd != null)
            {
                _joins.Add(joinToAdd);
            }
        }

        internal void AddJoins(List<QueryJoin> joinsToAdd, QueryFilterConfig filters)
        {
            if (joinsToAdd != null)
            {
                foreach (var join in joinsToAdd)
                {
                    if (join.ConditionOn != null && filters != null)
                    {
                        if (filters.Parameters.Any((f) => f.Key.ToLower() == join.ConditionOn.ToLower()))
                        {
                            AddJoin(join);
                        }
                    } else
                    {
                        AddJoin(join);
                    }
                }
            }
        }

        public string GetJoinSyntax()
        {
            return _joinSql;
        }

        public string GetFieldSyntax()
        {
            return _fieldSql;
        }

        public void SetAlias(string alias)
        {
            _alias = alias;
        }

        public string GetAlias()
        {
            return _alias;
        }

        public List<QueryJoin> GetListJoins()
        {
            return _listJoins;
        }

        internal void CreateJoinSyntax(string listItems, string multiSelectItems, List<string> tableFields, QueryFilterConfig filters)
        {
            var alias = "a";

            if (_joins != null && _joins.Count > 0)
            {
                foreach (var join in _joins)
                {
                    _joinSql += join.GetJoinSyntax(alias, filters) + " ";
                    var fieldList = join.GetFieldList(alias);

                    if (! string.IsNullOrWhiteSpace(fieldList))
                    {
                        _fieldSql += _fieldSql.Length > 0 ? ", " + fieldList : fieldList;
                    }

                    // A joined table keeps some coded values in columns and the rest in MoreData, so the
                    // list item join needs that table's own column list to pick the right one.
                    AddListItemJoins(join.ListItems, multiSelectItems, alias, GetColumnsFor(join.Table), join.Table);

                    alias = GetNextAlias(_alias);
                    _alias = alias;
                }
            }
            _alias = alias;
            AddListItemJoins(listItems, multiSelectItems, "s", tableFields);
        }

        /// <summary>
        /// Returns the real column names of a table, so a value held in MoreData can be told apart from one
        /// held in a column of its own.
        /// </summary>
        private static List<string> GetColumnsFor(string tableName)
        {
            var moreData = new GenerateMoreData(new JsonWholeStructureFieldsCollector(), new JsonElementRemover());
            return moreData.GetFieldList(tableName);
        }

        public void AddListItemJoins(string listItems, string multiSelectItems, string joinAlias = "z", List<string> tableFields = null, string sourceTable = null)
        {
            joinAlias = joinAlias == "z" ?  _alias : joinAlias;
            tableFields ??= new List<string>();
            sourceTable = string.IsNullOrWhiteSpace(sourceTable) ? _tableName : sourceTable;

            multiSelectItems = string.IsNullOrWhiteSpace(multiSelectItems) ? "" : multiSelectItems;
            var multiSelectList = multiSelectItems.Split(",").ToList();

            if (!string.IsNullOrEmpty(listItems))
            {
                foreach (var listItem in listItems.Split(","))
                {
                    var newField = listItem.Trim() + "Id";
                    var isMultiSelect = multiSelectList.Any(m => m.Trim().ToLower() == listItem.Trim().ToLower());
                    var heldInMoreData = tableFields.Count > 0 && !tableFields.Contains(newField.ToLower());

                    // The aggregate below runs over the source table under its own alias, so the multi-select
                    // column is named against that alias rather than the alias used in the outer query.
                    var aggregatedField = heldInMoreData ? "s.moredata::jsonb->>'" + newField + "'" : "s." + newField;

                    if (heldInMoreData)
                    {
                        newField = isMultiSelect
                            ? joinAlias + ".moredata::jsonb->>'" + newField + "'"
                            : "cast(nullif(" + joinAlias + ".moredata::jsonb->>'" + newField + "', '') As int)";
                    } else
                    {
                        newField = joinAlias + "." + listItem.Trim() + "Id";
                    }

                    _alias = GetNextAlias(_alias);

                    if (isMultiSelect) {
                        _joinSql += @"left outer join
                                     (
                                        select s.Id, string_agg(li.Value, ', ') As Value from " + sourceTable + @" s
                                        left outer join ListItem li on li.Id = any(string_to_array(" + aggregatedField + @", ',')::int[])
                                        group by s.Id
                                     ) " + _alias + " on " + _alias + ".Id = " + joinAlias + ".Id ";
                    } else
                    {
                        _joinSql += "left outer join ListItem " + _alias + " on " + _alias + ".Id = " + newField + " ";
                    }
                    _fieldSql += _fieldSql.Length > 0 ? ", " + _alias + ".Value" + " As " + listItem.Trim() : _alias + ".Value" + " As " + listItem.Trim();

                    _listJoins.Add(new QueryJoin() { LastUsedAlias = _alias, Fields = [new() { Name = listItem }] });
                }
            }
        }


        public SqlStatement AddListItemJoinsNew(string listItems, string multiSelectItems, string tableName, SqlStatement sqlStatement)
        {
            var jsonConverter = new JsonWholeStructureFieldsCollector();
            var jsonRemover = new JsonElementRemover();
            var moreData = new GenerateMoreData(jsonConverter, jsonRemover);
            var tableFields = moreData.GetFieldList(tableName);

            multiSelectItems = string.IsNullOrWhiteSpace(multiSelectItems) ? "" : multiSelectItems;
            var multiSelectList = multiSelectItems.Split(",").ToList();

            if (!string.IsNullOrEmpty(listItems))
            {
                foreach (var listItem in listItems.Split(","))
                {
                    var newFieldName = listItem.Trim() + "Id";

                    var newField = new SqlField { 
                        FieldName = listItem.Trim() + "Id",
                        JsonField = "moredata", 
                        MultiSelect = multiSelectList.Any(m => m.Trim().Equals(listItem.Trim(), System.StringComparison.CurrentCultureIgnoreCase)),
                        TableName = tableName
                    };
                    sqlStatement.AddField(newField);
                }
            }
            return sqlStatement;
        }

        private string GetNextAlias(string current)
        {
            return _prefixHandler.GetNewPrefix(current);
        }
    }
}
