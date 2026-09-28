using arc.app.Common;
using System;

namespace arc.app.Images.Queries;

/// <summary>
/// Special query factory for image operations
/// </summary>
public class ImageSpecialQueryFactory : ISpecialQueryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ImageSpecialQueryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IQueryRun GetQuery(string queryName)
    {
        return queryName.ToLower() switch
        {
            "imagelistquery" => new ImageListQuery(_serviceProvider.GetService(typeof(IImageRepository)) as IImageRepository),
            "singleimagequery" => new SingleImageQuery(_serviceProvider.GetService(typeof(IImageRepository)) as IImageRepository),
            _ => null,
        };
    }
}
