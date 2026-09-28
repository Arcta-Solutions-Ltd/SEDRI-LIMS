using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for moving a field to another form group (including across pages).
    /// Uses movefieldquery for initial data including formGroupOptions.
    /// </summary>
    internal class MoveFieldFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'movefieldform',
                        viewTitle: '@ConMovFG@',
                        saveEvent: 'movefield',
                        initialquery: 'movefieldquery',
                        suppressRecordView: true,
                        pages: [ 'movefieldpage']
                    }";

            return form;
        }
    }
}
