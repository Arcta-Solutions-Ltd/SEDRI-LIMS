//using arc.app.Config;
//using arc.app.Config.Workflows;
//using arc.common;
//using arc.common.Models.Role;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.app.Common
//{
//    public class TestHandler : ITestHandler
//    {
//        private readonly IWorkflowAdapter _workflowAdapter;
//        private readonly IAllBaseViewConfigFactory _allBaseViewConfigFactory;

//        public TestHandler(IWorkflowAdapter workflowAdapter, IAllBaseViewConfigFactory allBaseViewConfigFactory)
//        {
//            _workflowAdapter = workflowAdapter;
//            _allBaseViewConfigFactory = allBaseViewConfigFactory;
//        }

//        public async Task<List<CraftedSelectionsModel>> GetDefaultTests(EventModel model, int specimenType) {
//            var view = _allBaseViewConfigFactory.Get(model.View);
//            if (view == null || view.Workflow == null) { return null; }

//            var workflow = await _workflowAdapter.GetWorkflow(view.Workflow);
//            var returnList = new List<CraftedSelectionsModel>();

//            var specimenTypeTests = workflow.DefaultTests.Where(t => t.SpecimenType == specimenType);
//            if (specimenTypeTests.Count() > 0)
//            {
//                var testlist = specimenTypeTests.First().Tests;
//                foreach (var test in testlist)
//                {
//                    var newItem = new CraftedSelectionsModel { Key = test, Name = test, Allowed = "Yes" };
//                    returnList.Add(newItem);
//                }
//            }

//            return returnList;
//        }

//        public List<CraftedSelectionsModel> CombineLists(List<CraftedSelectionsModel> listOne, List<CraftedSelectionsModel> listTwo)
//        {

//            var returnList = listOne == null ? new List<CraftedSelectionsModel>() : listOne;

//            if (listTwo != null)
//            {
//                foreach (var item in listTwo)
//                {
//                    if (! returnList.Any(t => t.Key.ToLower() == item.Key.ToLower()))
//                    {
//                        returnList.Add(item);
//                    }
//                }
//            }

//            return returnList;
//        }
//    }
//}
