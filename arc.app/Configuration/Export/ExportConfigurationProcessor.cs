using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.data.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace arc.app.Configuration.Export
{
    /// <summary>
    /// Creates the string which represents the data which must be exported from the system.
    /// </summary>
    public class ExportConfigurationProcessor : IExportConfigurationProcessor
    {
        private readonly IGeneralRepository _generalRepository;
        private readonly ILogWriter _logWriter;

        public ExportConfigurationProcessor(IGeneralRepository generalRepository, ILogWriter logWriter) 
        {
            _generalRepository = generalRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Creates the string which represents the data which must be exported from the system for the tables requested
        /// </summary>
        /// <param name="_configDefinitions">List of tables for which the data must be included in the return string</param>
        /// <returns>String containing the data</returns>
        public async Task<List<string>> GetDataAsync(Dictionary<string, IEnumerable<string>> _configDefinitions)
        {
            var tablesToCopy = DataModelUtils.DataModelList();

            var returnList = new List<string>();

            foreach ( var def in _configDefinitions )
            {
                foreach(var item in def.Value)
                {
                    _logWriter.LogInfo($"Get information for table {item}", "ConfigTransferHandler", "GetConfig");
                    var table = tablesToCopy.FirstOrDefault(d => d.Value == item);
                    var contents = await InvokeGetAllAsyncUsingReflectionAsync(table, _generalRepository);
                    returnList.AddRange(contents.ToDelimitedString("<|>", table.Value));
                    _logWriter.LogInfo($"Load of configuration information for table {item} completed", "ConfigTransferHandler", "GetConfig");
                }
            }

            return returnList;
        }

        /// <summary>
        /// Invokes the GetAllAsync method using reflection.
        /// </summary>
        /// <param name="tableToCopy">A KeyValuePair containing the Type and string value representing the table to copy.</param>
        /// <param name="repository">An instance of IGeneralRepository.</param>
        /// <returns>An IEnumerable of objects.</returns>
        private static async Task<IEnumerable<object>> InvokeGetAllAsyncUsingReflectionAsync(
            KeyValuePair<Type, string> tableToCopy,
            IGeneralRepository repository
        )
        {
            MethodInfo method = typeof(IGeneralRepository).GetMethod("GetAllAsync").MakeGenericMethod(tableToCopy.Key);
            Task task = (Task)method.Invoke(repository, new object[] { tableToCopy.Value });
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty.GetValue(task);

            return result as IEnumerable<object>;
        }

    }
}





