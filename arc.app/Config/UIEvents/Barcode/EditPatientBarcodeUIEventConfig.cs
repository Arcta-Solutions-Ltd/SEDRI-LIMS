using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditPatientBarcodeUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpatientbarcodeuievent',
                        description: 'Edit a label dimension and content',
                        type: 'form',
                        action: 'editpatientbarcodeform'
                    }";

            return newEvent;
        }
    }
}
