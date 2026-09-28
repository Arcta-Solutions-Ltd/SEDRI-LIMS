using arc.app.Common;
using arc.app.Config.Mapper.Tests;

namespace arc.app.Config.Mapper
{
    internal class TestsMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "activetestlistresultmapper" => new ActiveTestListResultMapper(),
                "apipaneltestmapper" => new APIPanelTestMapper(),
                "apipaneltestquerymapper" => new APIPanelTestQueryMapper(),
                "auraminetestmapper" => new AuramineTestMapper(),
                "auraminetestquerymapper" => new AuramineTestQueryMapper(),
                "betalactamasetestmapper" => new BetalactamaseTestMapper(),
                "betalactamasetestquerymapper" => new BetalactamaseTestQueryMapper(),
                "biochemistrytestmapper" => new BiochemistryTestMapper(),
                "biochemistrytestquerymapper" => new BiochemistryTestQueryMapper(),
                "carbapenemasetestmapper" => new CarbapenemaseTestMapper(),
                "carbapenemasetestquerymapper" => new CarbapenemaseTestQueryMapper(),
                "catalasetestmapper" => new CatalaseTestMapper(),
                "catalasetestquerymapper" => new CatalaseTestQueryMapper(),
                "cellcounttestmapper" => new CellCountTestMapper(),
                "cellcounttestquerymapper" => new CellCountTestQueryMapper(),
                "dipsticktestmapper" => new DipstickTestMapper(),
                "dipsticktestquerymapper" => new DipstickTestQueryMapper(),
                "esbltestmapper" => new EsblTestMapper(),
                "esbltestquerymapper" => new EsblTestQueryMapper(),
                "gramculturetestmapper" => new GramCultureTestMapper(),
                "gramculturetestquerymapper" => new GramCultureTestQueryMapper(),
                "gramstaintestmapper" => new GramStainTestMapper(),
                "gramstaintestquerymapper" => new GramStainTestQueryMapper(),
                "hpyloriantigentestmapper" => new HPyloriAntigenTestMapper(),
                "hpyloriantigentestquerymapper" => new HPyloriAntigenTestQueryMapper(),
                "indiainktestmapper" => new IndiaInkTestMapper(),
                "indiainktestquerymapper" => new IndiaInkTestQueryMapper(),
                "jevserologytestmapper" => new JEVSerologyTestMapper(),
                "jevserologytestquerymapper" => new JEVSerologyTestQueryMapper(),
                "kohpreptestmapper" => new KOHPrepTestMapper(),
                "kohpreptestquerymapper" => new KOHPrepTestQueryMapper(),
                "microscopytestmapper" => new MicroscopyTestMapper(),
                "microscopytestquerymapper" => new MicroscopyTestQueryMapper(),
                "oxidasetestmapper" => new OxidaseTestMapper(),
                "oxidasetestquerymapper" => new OxidaseTestQueryMapper(),
                "pregnancytestmapper" => new PregnancyTestMapper(),
                "pregnancytestquerymapper" => new PregnancyTestQueryMapper(),
                "testlistforcultureparametermapper" => new TestListForCultureParameterMapper(),
                "testlistforspecimenparametermapper" => new TestListForSpecimenParameterMapper(),
                "testlistforspecimenresultmapper" => new TestListForSpecimenResultMapper(),
                "testlistforspecimenliteresultmapper" => new TestListForSpecimenLiteResultMapper(),
                "testlistforcultureliteresultmapper" => new TestListForCultureLiteResultMapper(),
                "testlistitemresultmapper" => new TestListItemResultMapper(),
                "wetpreptestmapper" => new WetprepTestMapper(),
                "wetpreptestquerymapper" => new WetPrepTestQueryMapper(),
                "wrightsstaintestmapper" => new WrightsStainTestMapper(),
                "wrightsstaintestquerymapper" => new WrightsStainTestQueryMapper(),
                "znstaintestmapper" => new ZNStainTestMapper(),
                "znstaintestquerymapper" => new ZnStainTestQueryMapper(),
                _ => null,
            };
        }
    }
}
