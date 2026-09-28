using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class TestEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "apipaneltest" => new APIPanelTestEventConfig(),
                "auraminetest" => new AuramineTestEventConfig(),
                "betalactamasetest" => new BetalactamaseTestEventConfig(),
                "biochemistrytest" => new BiochemistryTestEventConfig(),
                "carbapenemasetest" => new CarbapenemaseTestEventConfig(),
                "catalasetest" => new CatalaseTestEventConfig(),
                "cellcounttest" => new CellCountTestEventConfig(),
                "culturetestentry" => new CultureTestEntryEventConfig(),
                "culturetestselection" => new CultureTestSelectionEventConfig(),
                "deleteculturetest" => new DeleteCultureTestEventConfig(),
                "dipsticktest" => new DipstickTestEventConfig(),
                "directtestentry" => new DirectTestEntryEventConfig(),
                "deletespecimentest" => new DeleteSpecimenTestEventConfig(),
                "esbltest" => new EsblTestEventConfig(),
                "gramculturetest" => new GramCultureTestEventConfig(),
                "gramstaintest" => new GramStainTestEventConfig(),
                "hpyloriantigentest" => new HPyloriantigenTestEventConfig(),
                "indiainktest" => new IndiaInkTestEventConfig(),
                "jevserologytest" => new JEVSerologyTestEventConfig(),
                "kohpreptest" => new KOHPrepTestEventConfig(),
                "microscopytest" => new MicroscopyTestEventConfig(),
                "oxidasetest" => new OxidaseTestEventConfig(),
                "pregnancytest" => new PregnancyTestEventConfig(),
                "testselection" => new TestSelectionEventConfig(),
                "wetpreptest" => new WetprepTestEventConfig(),
                "wrightsstaintest" => new WrightsStainTestEventConfig(),
                "znstaintest" => new ZNStainTestEventConfig(),
                _ => null,
            };
        }
    }
}
