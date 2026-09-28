using arc.common.Models.Alert;
using arc.common.Models.AST;
using arc.domain.Alert;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Tests;
using System.Collections.Generic;

namespace arc.app.Alert
{
    public interface IAlertProcessor
    {
        List<SpecimenAlert> ProcessTestList(List<Test> directTestList, List<Test> cultureTestList, List<AlertDetailsModel> alerts, int specimenId);
        List<SpecimenAlert> ApplyASTAlerts(List<AlertDetailsModel> alerts, List<ASTModel> astResults, int specimenId, int cultureId);
    }
}
