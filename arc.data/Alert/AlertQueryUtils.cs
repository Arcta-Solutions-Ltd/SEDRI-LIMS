using arc.common.Models.Alert;
using arc.common.Models.Coding;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class AlertQueryUtils
    {
        private NpgsqlConnection _connect;
        private int _organismId;
        private OrganismDescriptorModel _organism;

        internal AlertQueryUtils(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            _connect = connect;
            var organismValue = queryFilters.Parameters.First(p => p.Key.ToLower() == "organismid");
            _organismId = int.Parse(organismValue.Value);
        }

        internal async Task<List<AlertDetailsModel>> GetAlerts(bool includeAstAlerts = false, bool includeTestAlerts = false)
        {
            var sql = @"select o.id as organismid, o.genusid, o.speciesid, g.familyid, f.orderid, o.additionalid, o.serotypeid, o.subspeciesid
                        from Organism o
                        left outer join genus g on g.id = o.genusid
                        left outer join family f on f.id = g.familyid
                        where o.Id = @OrganismId";

            var queryResult = await _connect.QueryAsync<OrganismDescriptorModel>(sql, new { OrganismId = _organismId });
            _organism = queryResult.FirstOrDefault();

            sql = @"select distinct a.* from alert a
	                left outer join organism o on o.id = a.organismid
                    where doesexist = 'Yes' and Enabled = 'Yes' and
                    (a.orderid = @OrderId) and
                    (a.familyid = @FamilyId or a.familyid = 0 or a.familyid is null) and
                    (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                    (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and 
	                (o.subspeciesid = @SubspeciesId or o.subspeciesId is null or o.subspeciesId = 0) and
	                (o.serotypeid = @SerotypeId or o.serotypeId is null or o.serotypeId = 0) and
                    (o.additionalid = @AdditionalId or o.additionalid = 0 or o.additionalid is null) and
                    (a.orggroupcodingid is null or a.orggroupcodingid = 0) 
                    union		
                    select distinct a.* from alert a
	                inner join organismcoding oc on a.orggroupcodingid = oc.CodingId
                    left outer join organism o on o.id = oc.organismid
                    left outer join genus g on o.genusid = g.id
                    left outer join family f on g.familyid = f.id
                    where doesexist = 'Yes' and Enabled = 'Yes' and
                    (((f.orderid = @OrderId or f.orderid = 0 or f.orderid is null) and
                    (g.familyid = @FamilyId or g.familyid = 0 or g.familyid is null) and
                    (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                    (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and
                    (o.serotypeid = @SerotypeId or o.serotypeid = 0 or o.serotypeid is null) and
                    (o.subspeciesid = @SubspeciesId or o.subspeciesid = 0 or o.subspeciesid is null) and
                    (a.orggroupcodingid != 0) and (a.orggroupcodingid is not null))
                    or oc.OrganismId = @OrganismId)";

            if (includeAstAlerts)
            {
                if (includeTestAlerts)
                {
                    sql = AstAndTestAlertSql();
                } else
                {
                    sql = AstAlertSql();
                }
            } else
            {
                if (includeTestAlerts)
                {
                    sql = TestAlertSql();
                }
            }

            var alerts = await _connect.QueryAsync<AlertDetailsModel>(sql, _organism);

            if (includeAstAlerts)
            {
                foreach (var singleAlert in alerts)
                {
                    sql = @"select * from alertlines where alertid = @AlertId";
                    var astLines = await _connect.QueryAsync<SusceptibilityGrid>(sql, new { AlertId = singleAlert.Id });
                    singleAlert.SusceptibilityGrid = astLines.Select((t) => new SusceptibilityGridModel
                    { AntibioticId = t.AntibioticId, SusceptibilityId = t.SusceptibilityId }).ToList();
                }
            }

            if (includeTestAlerts)
            {
                foreach (var singleAlert in alerts)
                {
                    sql = @"select * from alerttestlines where alertid = @AlertId";
                    var alertTestLines = await _connect.QueryAsync<TestGrid>(sql, new { AlertId = singleAlert.Id });
                    singleAlert.TestGrid = alertTestLines.Select((t) => new TestGridModel
                    { Test = t.TestName, Field = t.FieldName, Comparison = t.Comparison, ListValue = t.CompValue, StringValue = t.CompValue, NumberValue = t.CompValue }).ToList();
                }
            }

            return alerts.ToList();
        }

        private string AstAlertSql()
        {
            return RootAlertSql("ast.total > 0 and tst.total is null");
        }

        private string AstAndTestAlertSql()
        {
            return RootAlertSql("ast.total > 0 and tst.total > 0");
        }

        private string TestAlertSql()
        {
            return RootAlertSql("ast.total is null and tst.total > 0");
        }

        private string RootAlertSql (string whereClause)
        {
            var sql = @"with astlines as (select alertid, count(*) as total from alertlines group by alertid),
                        testlines as (select alertid, count(*) as total from alerttestlines group by alertid)
                        select distinct a.* from alert a
	                    left outer join organism o on o.id = a.organismid
	                    left outer join genus g on o.genusid = g.id 
                        left outer join astlines ast on a.Id = ast.alertid
                        left outer join testlines tst on a.Id = tst.alertid
                        where doesexist = 'No' and " + whereClause + @" and Enabled = 'Yes' and
                        (a.orderid = @OrderId) and
                        (a.familyid = @FamilyId or a.familyid = 0 or a.familyid is null) and
                        (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                        (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and 
	                    (o.subspeciesid = @SubspeciesId or o.subspeciesId is null or o.subspeciesId = 0) and
	                    (o.serotypeid = @SerotypeId or o.serotypeId is null or o.serotypeId = 0) and
                        (o.additionalid = @AdditionalId or o.additionalid = 0 or o.additionalid is null) and
                        (a.orggroupcodingid is null or a.orggroupcodingid = 0)
                        union
                        select distinct a.* from alert a
	                    inner join organismcoding oc on a.orggroupcodingid = oc.CodingId
                        left outer join organism o on o.id = oc.organismid
                        left outer join genus g on o.genusid = g.id
                        left outer join family f on g.familyid = f.id
                        left outer join astlines ast on a.Id = ast.alertid
                        left outer join testlines tst on a.Id = tst.alertid
                        where doesexist = 'No' and " + whereClause + @" and Enabled = 'Yes' and
                        (((f.orderid = @OrderId or f.orderid = 0 or f.orderid is null) and
                        (g.familyid = @FamilyId or g.familyid = 0 or g.familyid is null) and
                        (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                        (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and
                        (o.serotypeid = @SerotypeId or o.serotypeid = 0 or o.serotypeid is null) and
                        (o.subspeciesid = @SubspeciesId or o.subspeciesid = 0 or o.subspeciesid is null) and
                        (a.orggroupcodingid != 0) and (a.orggroupcodingid is not null))
                        or oc.OrganismId = @OrganismId)";

            return sql;
        }
    }
}
