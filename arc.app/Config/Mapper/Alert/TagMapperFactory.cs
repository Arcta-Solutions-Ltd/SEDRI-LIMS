using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TagMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addtagmapper" => new AddTagMapper(),
                "alertviewmapper" => new AlertViewMapper(),
                "alerttypeusedinalertmapper" => new AlertTypeUsedInAlertMapper(),
                "edittagmapper" => new EditTagMapper(),
                "taghaschildrenmapper" => new TagHasChildrenMapper(),
                "tagexistsmapper" => new TagExistsMapper(),
                _ => null,
            };
        }
    }
}
