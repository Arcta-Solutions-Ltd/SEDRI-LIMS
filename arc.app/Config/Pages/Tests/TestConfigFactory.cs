using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class TestConfigFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "apipaneltestpage" => new APIPanelTestPageConfig(),
                "auraminetestpage" => new AuramineTestPageConfig(),
                "betalactamasetestpage" => new BetalactamaseTestPageConfig(),
                "biochemistrytestpage" => new BiochemistryTestPageConfig(),
                "carbapenemasetestpage" => new CarbapenemaseTestPageConfig(),
                "catalasetestpage" => new CatalaseTestPageConfig(),
                "cellcounttestpage" => new CellCountTestPageConfig(),
                "culturetestpage" => new CultureTestPageConfig(),
                "dipsticktestpage" => new DipstickTestPageConfig(),
                "directtestpage" => new DirectTestPageConfig(),
                "esbltestpage" => new EsblTestPageConfig(),
                "gramculturetestpage" => new GramCultureTestPageConfig(),
                "gramstaintestpage" => new GramStainTestPageConfig(),
                "hpyloriantigentestpage" => new HPyloriAntigenTestPageConfig(),
                "indiainktestpage" => new IndiaInkTestPageConfig(),
                "jevserologytestpage" => new JEVSerologyTestPageConfig(),
                "kohpreptestpage" => new KOHPrepTestPageConfig(),
                "microscopytestpage" => new MicroscopyTestPageConfig(),
                "oxidasetestpage" => new OxidaseTestPageConfig(),
                "pregnancytestpage" => new PregnancyTestPageConfig(),
                "testcultureselectionpage" => new TestCultureSelectionPageConfig(),
                "testselectionpage" => new TestSelectionPageConfig(),
                "wetpreptestpage" => new WetPrepTestPageConfig(),
                "wrightsstaintestpage" => new WrightsStainTestPageConfig(),
                "znstaintestpage" => new ZNStainTestPageConfig(),
                _ => null,
            };
        }


    }
}
