using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSpecimenBarcodeUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editspecimenbarcodeuievent',
                        description: 'Edit a label dimension and content',
                        type: 'form',
                        action: 'editspecimenbarcodeform'
                    }";

            return newEvent;
        }
    }
}
