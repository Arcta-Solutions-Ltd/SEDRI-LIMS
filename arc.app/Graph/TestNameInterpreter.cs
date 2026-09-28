using arc.app.Config.Forms;
using arc.common.Models.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Graph
{
    public class TestNameInterpreter : ITestNameInterpreter
    {
        private readonly IFormConfigAdapter _formConfigAdapter;

        public TestNameInterpreter(IFormConfigAdapter formConfigAdapter)
        {
            _formConfigAdapter = formConfigAdapter;
        }

        public async Task<List<GraphModel>> InterpretAsync(List<GraphModel> rows)
        {
            var testList = new Dictionary<string, string>();

            foreach (var row in rows)
            {
                if (testList.ContainsKey(row.Value)) {
                    row.Value = testList[row.Value];
                } else
                {
                    var testForm = await _formConfigAdapter.GetFormAsync(row.Value);
                    testList.Add(row.Value, testForm.Title);
                    row.Value = testForm.Title;
                }
            }

            return rows;
        }
    }
}
