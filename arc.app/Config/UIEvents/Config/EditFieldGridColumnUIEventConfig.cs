using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditFieldGridColumnUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editfieldgridcolumnuievent',
                        description: 'Edit grid column',
                        type: 'form',
                        action: 'editfieldgridcolumnform'
                    }";

            return newEvent;
        }
    }
}
