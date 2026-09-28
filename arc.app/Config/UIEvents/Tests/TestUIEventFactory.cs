using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class TestUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "apipaneltestuievent" => new APIPanelTestUIEventConfig(),
                "auraminetestuievent" => new AuramineTestUIEventConfig(),
                "betalactamasetestuievent" => new BetalactamaseUIEventConfig(),
                "biochemistrytestuievent" => new BiochemistryTestUIEventConfig(),
                "carbapenemasetestuievent" => new CarbapenemaseUIEventConfig(),
                "catalasetestuievent" => new CatalaseTestUIEventConfig(),
                "cellcounttestuievent" => new CellCountTestUIEventConfig(),
                "culturetestuievent" => new CultureTestUIEventConfig(),
                "culturetestselectionuievent" => new CultureTestSelectionUIEvent(),
                "dipsticktestuievent" => new DipstickTestUIEventConfig(),
                "directtestuievent" => new DirectTestUIEventConfig(),
                "esbltestuievent" => new EsblTestUIEventConfig(),
                "gramculturetestuievent" => new GramCultureTestUIEventConfig(),
                "gramstaintestuievent" => new GramStainTestUIEventConfig(),
                "jevserologytestuievent" => new JEVSerologyTestUIEventConfig(),
                "indiainktestuievent" => new IndiaInkTestUIEventConfig(),
                "hpyloriantigentestuievent" => new HPyloriantigenTestUIEventConfig(),
                "kohpreptestuievent" => new KOHPrepTestUIEventConfig(),
                "microscopytestuievent" => new MicroscopyTestUIEventConfig(),
                "oxidasetestuievent" => new OxidaseTestUIEventConfig(),
                "pregnancytestuievent" => new PregnancyTestUIEventConfig(),
                "testselectionuievent" => new TestSelectionUIEventConfig(),
                "wetpreptestuievent" => new WetPrepTestUIEventConfig(),
                "wrightsstaintestuievent" => new WrightsStainTestUIEventConfig(),
                "znstaintestuievent" => new ZNStainTestUIEventConfig(),
                _ => null,
            };
        }
    }
}
