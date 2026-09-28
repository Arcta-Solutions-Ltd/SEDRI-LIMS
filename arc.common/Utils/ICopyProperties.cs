namespace arc.common.Utils
{
    public interface ICopyProperties
    {
        void CopyAll<S, T>(S source, T target);
    }
}
