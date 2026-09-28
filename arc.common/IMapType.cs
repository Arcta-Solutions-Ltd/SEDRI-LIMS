namespace arc.common;

public interface IMapType<TSource, TTarget>
{
    TTarget Map(TSource source);
}
