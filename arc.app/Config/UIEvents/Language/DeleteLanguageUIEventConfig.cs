using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteLanguageUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletelanguageuievent',
                        description: 'Delete Language',
                        type: 'form',
                        action: 'deletelanguageform'
                    }";

            return newEvent;
        }
    }
}
