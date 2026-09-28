namespace arc.app.Configuration
{
    public interface IConfigFactory
    {
        ISingleConfig Create(string name);
    }
}
