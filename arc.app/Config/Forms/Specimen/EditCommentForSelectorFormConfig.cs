using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Forms.Specimen
{
    internal class EditCommentForSelectorFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'editcommentforselectorform',
                            formtype: 'singlepage',
                            saveEvent: 'editcomment',
                            suppressRecordView: true,
                            initialQuery: 'editcommentquery',
                            pages: [ 'editcommentforselectorpage' ] 
                        }";

            return form;
        }
    }
}
