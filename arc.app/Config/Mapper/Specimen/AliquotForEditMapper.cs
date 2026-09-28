using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AliquotForEditMapper : IDefinition
    {
        public string Get()
        {
            return @"{                      
                          Name: 'aliquotforeditmapper',
                          Type: 'Standard',
                          Rules: [
                            {
                              Key: '<:1:>',
                              Type: 'Mapping',
                              Value: 'AloquatId',
                              Source: 'AloquatId'
                            }
                          ],
                          Target: {
                            AliquotID: '<:1:>'
                          }
                    }";
        }
    }
}
