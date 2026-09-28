using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class TestFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                //case "apipaneltestform":
                //    return new APIPanelTestFormConfig();
                //case "auraminetestform":
                //    return new AuramineTestFormConfig();
                //case "betalactamasetestform":
                //    return new BetalactamaseTestFormConfig();
                //case "biochemistrytestform":
                //    return new BiochemistryTestFormConfig();
                //case "carbapenemasetestform":
                //    return new CarbapenemaseTestFormConfig();
                "cellcounttestform" => new CellCountTestFormConfig(),
                "culturetestform" => new CultureTestFormConfig(),
                "culturetestselectionform" => new CultureTestSelectionFormConfig(),
                //case "dipsticktestform":
                //    return new DipStickTestFormConfig();
                "directtestform" => new DirectTestFormConfig(),
                //case "esbltestform":
                //    return new EsblTestFormConfig();
                "gramstaintestform" => new GramStainTestFormConfig(),
                //case "hpyloriantigentestform":
                //    return new HPyloriAntigenTestFormConfig();
                //case "indiainktestform":
                //    return new IndiaInkTestFormConfig();
                //case "jevserologytestform":
                //    return new JEVSerologyTestFormConfig();
                //case "kohpreptestform":
                ////    return new KOHPrepTestFormConfig();
                //case "microscopytestform":
                //    return new MicroscopyTestFormConfig();
                "pregnancytestform" => new PregnancyTestFormConfig(),
                "testselectionform" => new TestSelectionFormConfig(),
                "znstaintestform" => new ZNStainTestFormConfig(),
                _ => null
            };
        }
    }
}
