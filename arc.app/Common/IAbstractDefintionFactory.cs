namespace arc.app.Common
{
    public interface IAbstractDefinitionFactory
    {
        IDefinitionFactory Create(string abstractFactoryName);
    }
}
