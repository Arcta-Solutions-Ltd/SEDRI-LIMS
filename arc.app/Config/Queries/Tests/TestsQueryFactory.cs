using arc.app.Common;
using arc.app.Config.Queries.Tests;

namespace arc.app.Config.Queries
{
    public class TestsQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "activeculturetestlistquery" => new ActiveCultureTestListQuery(),
                "activeculturetestbyidfortestlistquery" => new ActiveCultureTestByIdForTestListQuery(),
                "activetestbyidfortestlistquery" => new ActiveTestByIdForTestListQuery(),
                "activetestlistquery" => new ActiveTestListQuery(),
                "auraminetestbyid" => new AuramineTestByIdQuery(),
                "betalactamasetestbyid" => new BetalactamaseTestByIdQuery(),
                "biochemistrytestbyid" => new BiochemistryTestByIdQuery(),
                "blankalltestselection" => new BlankAllTestSelectionQuery(),
                "blankalltestselectionwithpatientref" => new BlankAllTestSelectionWithPatientRefQuery(),
                "blankculturetestselection" => new BlankCultureTestSelectionQuery(),
                "blankculturetypeandtestselection" => new BlankCultureTypeAndTestSelectionQuery(),
                "blankculturetypeandtestselectionwithpatientref" => new BlankCultureTypeAndTestSelectionWithPatientRefQuery(),
                "blanktestselection" => new BlankTestSelectionQuery(),
                "carbapenemasetestbyid" => new CarbapenemaseTestByIdQuery(),
                "catalasetestbyid" => new CatalaseTestByIdQuery(),
                "cellcounttestbyid" => new CellCountTestByIdQuery(),
                "culturetestusagecountquery" => new CultureTestUsageCountQuery(),
                "dipsticktestbyid" => new DipstickTestByIdQuery(),
                "directtestusagecountquery" => new DirectTestUsageCountQuery(),
                "esbltestbyid" => new EsblTestByIdQuery(),
                "getorcreateculturetestid" => new GetOrCreateCultureTestIdQuery(),
                "gramculturetestbyid" => new GramCultureTestByIdQuery(),
                "gramstaintestbyid" => new GramStainTestByIdQuery(),
                "hpyloriantigentestbyid" => new HPyloriAntigenTestByIdQuery(),
                "indiainktestbyid" => new IndiaInkTestByIdQuery(),
                "jevserologytestbyid" => new JEVSerologyTestByIdQuery(),
                "kohpreptestbyid" => new KOHPrepTestByIdQuery(),
                "microscopytestbyid" => new MicroscopyTestByIdQuery(),
                "oxidasetestbyid" => new OxidaseTestByIdQuery(),
                "pregnancytestbyid" => new PregnancyTestByIdQuery(),
                "testlistforculture" => new TestListForCultureQuery(),
                "testlistforculturelite" => new TestListForCultureLiteQuery(),
                "testlistforspecimen" => new TestListForSpecimenQuery(),
                "testlistforspecimenlite" => new TestListForSpecimenLiteQuery(),
                "testrecordviewbyid" => new TestRecordViewByIdQuery(),
                "testselection" => new TestSelectionQuery(),
                "wetpreptestbyid" => new WetprepTestByIdQuery(),
                "wrightsstaintestbyid" => new WrightsStainTestByIdQuery(),
                "znstaintestbyid" => new ZNStainTestByIdQuery(),
                "apipaneltestbyid" => new APIPanelTestByIdQuery(),
                _ => null,
            };
        }
    }
}
