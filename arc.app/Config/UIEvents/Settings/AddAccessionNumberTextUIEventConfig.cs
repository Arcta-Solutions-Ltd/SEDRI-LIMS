using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddAccessionNumberTextUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addaccessionnumbertextuievent',
                        description: 'Add Accession Number Text',
                        type: 'form',
                        action: 'addaccessionnumbertextform'
                    }";

            return newEvent;
        }
    }
}
