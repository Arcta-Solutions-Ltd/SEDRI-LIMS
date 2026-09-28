using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditPatientCollectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpatientcollection',
                        description: 'Edit patient collection details',
                        type: 'form',
                        action: 'editpatientcollectionform'
                    }";

            return newEvent;
        }
    }
}
