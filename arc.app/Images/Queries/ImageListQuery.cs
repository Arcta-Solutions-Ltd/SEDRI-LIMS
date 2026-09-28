using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Images.Queries;

/// <summary>
/// Query to get the list of all active images
/// </summary>
public class ImageListQuery : IQueryRun
{
    private readonly IImageRepository _imageRepository;

    public ImageListQuery(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var images = await _imageRepository.GetImageListAsync();
        return JsonConvert.SerializeObject(images);
    }
}
