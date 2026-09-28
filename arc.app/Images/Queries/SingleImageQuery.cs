using arc.app.Common;
using arc.app.Images;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Images.Queries;

/// <summary>
/// Query to get a single image by ID
/// </summary>
public class SingleImageQuery : IQueryRun
{
    private readonly IImageRepository _imageRepository;

    public SingleImageQuery(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var idParam = queryFilter.Parameters.Where(p => p.Key.ToLower() == "id").FirstOrDefault();
        if (idParam == null || !int.TryParse(idParam.Value, out var id) || id <= 0)
        {
            return JsonConvert.SerializeObject(new { error = "Invalid or missing id parameter" });
        }

        var image = await _imageRepository.GetImageByIdAsync(id);
        return JsonConvert.SerializeObject(image);
    }
}
