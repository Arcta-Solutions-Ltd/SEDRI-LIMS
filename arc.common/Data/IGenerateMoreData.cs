using arc.common.Models.Config;
using System.Collections.Generic;

namespace arc.common.Data
{ 
    public interface IGenerateMoreData
    {
        string GetMoreDataJsonString(string table, string message, List<TableExceptionModel> tableExceptions = null);
        string GetExceptionListString(string table, string message, List<TableExceptionModel> tableExceptions = null);
        List<string> GetFieldList(string tableName);
        string GetLeftOverData();
    }
}
