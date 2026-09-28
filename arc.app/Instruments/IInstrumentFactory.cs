namespace arc.app.Instruments
{
    public interface IInstrumentFactory
    {
        IRespond Get(string type);
    }
}
