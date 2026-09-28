using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AliquotForEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                          Type: 'Single',
                          Query: 'aliquotforeditquery',
                          Where: [
                            {
                              Field: 'Id',
                              Comparison: '='
                            }
                          ],
                          Fields: [
                            {
                              Name: 'AloquatId',
                              Type: 'string'
                            }
                          ],
                          TableName: 'Culture',
                          ResultMapping: 'aliquotforeditmapper'
                        }";

        }
    }
}
