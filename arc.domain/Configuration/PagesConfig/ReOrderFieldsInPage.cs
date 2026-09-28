using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig
{
    internal class ReOrderFieldsInPage
    {
        private List<ColumnConfig> _columns;

        internal ReOrderFieldsInPage(List<ColumnConfig> columns)
        {
            _columns = columns;
        }

        internal ColumnConfig ReorderFields(List<string> newFieldListOrder)
        {
            var newFieldList = new List<FieldConfigWithRuleModel>();
            foreach(var field in newFieldListOrder)
            {
                newFieldList.Add(GetFieldInfo(field));
            }

            var returnColumn = _columns.First();
            returnColumn.DeleteAllFormGroups();

            var formGroupNumber = 1;
            var fieldNumber = 1;
            var currentFormGroup = new FormGroupConfig();

            foreach(var field in newFieldList)
            {
                if (fieldNumber == 1 || field.FormGroupRules != null)
                {
                    currentFormGroup = new FormGroupConfig { Key = "fg" + formGroupNumber, Rules = field.FormGroupRules };
                    returnColumn.AddFormGroup(currentFormGroup);
                    formGroupNumber++;
                }

                currentFormGroup.AddField(field.Field);
                fieldNumber++;
            }

            return returnColumn;
        }

        internal FieldConfigWithRuleModel GetFieldInfo(string fieldName)
        {
            var newConfig = new FieldConfigWithRuleModel();

            for (int colNum = 0; colNum < _columns.Count; colNum++)
            {
                for (int groupNum = 0; groupNum < _columns[colNum].FormGroups.Count; groupNum++)
                {
                    for (int fieldNum = 0; fieldNum < _columns[colNum].FormGroups[groupNum].Fields.Count; fieldNum++)
                    {
                        if (_columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id == fieldName)
                        {
                            newConfig.Field = _columns[colNum].FormGroups[groupNum].Fields[fieldNum];
                            newConfig.FormGroupRules = _columns[colNum].FormGroups[groupNum].Rules;
                        }
                    }
                }
            }
            return newConfig;
        }
    }

    internal class FieldConfigWithRuleModel
    {
        internal List<RuleConfig> FormGroupRules;
        internal FieldConfig Field;

    }
}
