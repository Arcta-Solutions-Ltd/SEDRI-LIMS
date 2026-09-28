using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditAliquotMapper : IDefinition
    {
        public string Get()
        {
            return @"{
                          Name: 'editaliquotmapper',
                          Type: 'Standard',
                          Rules: [
                            {
                              Key: '<:1:>',
                              Type: 'Mapping',
                              Value: 'Id',
                              Source: 'Id'
                            },
                            {
                              Key: '<:2:>',
                              Type: 'Mapping',
                              Value: 'AliquotID',
                              Source: 'AliquotID'
                            }
                          ],
                          Target: {
                            Id: '<:1:>',
                            AloquatId: '<:2:>'
                          }
                    }";
        }
    }
}
