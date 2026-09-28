using arc.app.Config.Views.DiaryViews;
using arc.app.Monitoring;
using arc.app.Roles;
using arc.app.Security;
using arc.app.SystemConfig;
using arc.app.Tests;
using arc.common;
using arc.domain.Tests;

namespace arc.app.Common
{
    public class SpecialMappingFactory : ISpecialMappingFactory
    {
        private readonly IAddRoleMapping _addRoleMapping;
        private readonly IAddTopLevelItemsToMenuPermissions _addTopLevelItemsToMenuPermissions;
        private readonly IEnsureMandatoryMenuPermissions _ensureMandatoryMenuPermissions;
        private readonly IDiaryFactory _diaryFactory;
        private readonly ITestResultsForListFormatter _testResultsForListFormatter;
        private readonly IConfigItemHandler _configItemHandler;

        public SpecialMappingFactory(IAddRoleMapping addRoleMapping, IAddTopLevelItemsToMenuPermissions addTopLevelItemsToMenuPermissions,
            IEnsureMandatoryMenuPermissions ensureMandatoryMenuPermissions, IDiaryFactory diaryFactory,
            ITestResultsForListFormatter testResultsForListFormatter, IConfigItemHandler configItemHandler)
        {
            _addRoleMapping = addRoleMapping;
            _addTopLevelItemsToMenuPermissions = addTopLevelItemsToMenuPermissions;
            _ensureMandatoryMenuPermissions = ensureMandatoryMenuPermissions;
            _diaryFactory = diaryFactory;
            _testResultsForListFormatter = testResultsForListFormatter;
            _configItemHandler = configItemHandler;
        }

        public IMap GetMapper(string mappingName)
        {
            return mappingName.ToLower() switch
            {
                "activetestlistresultmapper" => new TestListForSpecimenResultMapper<TestList>(_testResultsForListFormatter, _configItemHandler),
                "addrolemapper" => (IMap)_addRoleMapping,
                "menupermissionsmapper" => new MenuPermissionsMapping(_addTopLevelItemsToMenuPermissions, _ensureMandatoryMenuPermissions),
                "eventpermissionsmapper" => new EventPermissionsMapping(),
                "patientdiaryresultmapper" => new QueueToDiaryMapper("patientdiary", _diaryFactory),
                "specimendiaryresultmapper" => new QueueToDiaryMapper("specimendiary", _diaryFactory),
                "testlistitemresultmapper" => new TestListItemResultMapper(_testResultsForListFormatter, _configItemHandler),
                "testlistforspecimenresultmapper" => new TestListForSpecimenResultMapper<Test>(_testResultsForListFormatter, _configItemHandler),
                "testlistforspecimenliteresultmapper" => new TestListLiteResultMapper(_configItemHandler),
                "testlistforcultureresultmapper" => new TestListForSpecimenResultMapper<Test>(_testResultsForListFormatter, _configItemHandler),
                "testlistforcultureliteresultmapper" => new TestListLiteResultMapper(_configItemHandler),
                _ => null,
            };
        }
    }
}
