namespace arc.app.Common
{
    public interface IQueryHandlerFactory
    {
        IQueryBase Create(string type);
    }
}
