using arc.common.Models.AST;
using arc.common.Models.Coding;
using System.Collections.Generic;

namespace arc.common;
public interface IMapWithList<TSource, TTarget> : IMapType<TSource, TTarget>
{
    List<ASTRowModel> MapList(List<TSource> source);
}
