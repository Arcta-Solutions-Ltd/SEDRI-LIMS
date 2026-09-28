using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddFieldGridColumnUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addfieldgridcolumnuievent',
                        description: 'Add grid column',
                        type: 'form',
                        action: 'addfieldgridcolumnform'
                    }";

            return newEvent;
        }
    }
}
