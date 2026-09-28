using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    internal class ResolveOrganismScope<T> where T : Organism
    {
        internal async Task<T> Resolve (NpgsqlConnection connect, T command, T current )
        {
            if (command.OrganismId > 0 && command.OrganismId != current.OrganismId)
            {
                var hierachyUtil = new GetHierarchyFromOrganismId();
                var hierarchy = await hierachyUtil.ExecuteAsync(connect, new QueryFilterConfig().AddInteger("Id", command.OrganismId));
                command.OrderId = hierarchy.OrderId;
                command.FamilyId = hierarchy.FamilyId;
                command.OrgGroupCodingId = 0;
            }
            else
            {
                if ((command.OrderId > 0 && command.OrderId != current.OrderId) || command.FamilyId > 0 && command.FamilyId != current.FamilyId)
                {
                    command.OrganismId = 0;
                    command.OrgGroupCodingId = 0;
                }
                else
                {
                    if (command.OrgGroupCodingId > 0 && command.OrgGroupCodingId != current.OrgGroupCodingId)
                    {
                        command.OrganismId = 0;
                        command.OrderId = 0;
                        command.FamilyId = 0;
                    }
                    else
                    {
                        if (command.OrganismId > 0) {
                            var sql = @"select o.id as organismid, o.genusid, o.speciesid, g.familyid, f.orderid, o.additionalid, o.serotypeid, o.subspeciesid
                                        from Organism o
                                        left outer join genus g on g.id = o.genusid
                                        left outer join family f on f.id = g.familyid
                                        where o.Id = @OrganismId";

                            var queryResult = await connect.QueryAsync<OrganismDescriptorModel>(sql, new { command.OrganismId });
                            var org = queryResult.FirstOrDefault();

                            if ((command.GenusId > 0 && command.GenusId != org.GenusId) || (command.SpeciesId > 0 && command.SpeciesId != org.SpeciesId) || (command.SubSpeciesId > 0 && command.SubSpeciesId != org.SubspeciesId) || (command.SerotypeId > 0 && command.SerotypeId != org.SerotypeId))
                            {
                                var organismFinder = new GetOrganismIdFromHierarchy(connect);

                                command.OrganismId = await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
                            }
                        } else
                        {
                            var organismFinder = new GetOrganismIdFromHierarchy(connect);
                            command.OrganismId = await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
                        }
                    }
                }
            }

            return command;
        }
    }
}
