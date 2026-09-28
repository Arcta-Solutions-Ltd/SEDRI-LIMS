using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditPagesUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpagesuievent',
                        description: 'Edit Pages',
                        type: 'form',
                        action: 'editpagesform'
                    }";

            return newEvent;
        }
    }
}
