using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.UIEvents
{
    internal class EditAliquotUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editaliquotuievent',
                        description: '@SpeAliA@',
                        type: 'form',
                        action: 'editaliquotform'
                    }";

            return newEvent;
        }
    }
}
