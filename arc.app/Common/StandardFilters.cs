using arc.app.Location;
using arc.app.Security;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common;

public class StandardFilters : IStandardFilters
{
    private readonly IOrganisationRepository _organisationRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IListRepository _listRepository;

    public StandardFilters(IOrganisationRepository organisationRepository, ILocationRepository locationRepository, IListRepository listRepository)
    {
        _organisationRepository = organisationRepository;
        _locationRepository = locationRepository;
        _listRepository = listRepository;
    }

    public async Task<TokenInfoModel> UpdateTokenAsync(TokenInfoModel token)
    {
        if (token != null && !string.IsNullOrWhiteSpace(token.OrganisationId))
        {
            token.OrganisationId = await _organisationRepository.GetOrganisationHierarchyAsync(token.OrganisationId);
        }
        return token;
    }

    public async Task<QueryFilterConfig> UpdateFilterAsync(QueryFilterConfig queryFilters)
    {
        if (queryFilters.Parameters != null)
        {
            var locationSearchList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "locationid");
            if (locationSearchList.Any())
            {
                var locationId = locationSearchList.First().Value;
                locationSearchList.First().Value = await _locationRepository.GetLocationHierarchyAsync(locationId);
            }

            var tagSearchList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "tagid");
            if (tagSearchList.Any() && !string.IsNullOrWhiteSpace(tagSearchList.First().Value))
            {
                var tagId = tagSearchList.First().Value;
                tagSearchList.First().Value = await _listRepository.GetTagHierarchyAsync(tagId);
            }
        }

        return queryFilters;
    }

    //public async Task<QueryFilterConfig> ApplyTokenToQueryFiltersAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
    //{
    //    if (token != null && !string.IsNullOrWhiteSpace(token.OrganisationId))
    //    {
    //        queryFilters.AddString("OrganisationId", token.OrganisationId);
    //    }

    //    if (token != null && !string.IsNullOrWhiteSpace(token.LaboratoryId))
    //    {
    //        queryFilters.AddString("LaboratoryId", token.LaboratoryId);
    //    }

    //    return queryFilters;
    //}

}
