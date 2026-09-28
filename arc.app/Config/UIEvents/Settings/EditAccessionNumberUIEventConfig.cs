using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditAccessionNumberUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editaccessionnumberuievent',
                        description: 'Edit Accession Number',
                        type: 'form',
                        action: 'editaccessionnumberform'
                    }";

            return newEvent;
        }
    }
}
