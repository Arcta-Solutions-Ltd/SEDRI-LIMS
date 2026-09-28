using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.AST;
using arc.common.ExtensionMethods;
using arc.data.Configuration;
using arc.data.model.AST;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace arc.data.AST;

/// <summary>
/// Reads and writes astsusceptibilityoverride rows keyed by culture and stable line ids.
/// </summary>
public class AstSusceptibilityOverrideRepository : IAstSusceptibilityOverrideRepository
{
    private readonly IOptionsMonitor<DataOptions> _options;
    private readonly ILogger<AstSusceptibilityOverrideRepository> _logger;

    public AstSusceptibilityOverrideRepository(
        IOptionsMonitor<DataOptions> options,
        ILogger<AstSusceptibilityOverrideRepository> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AstSusceptibilityOverrideDataModel>> GetByCultureIdAsync(int cultureId)
    {
        await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        const string sql = @"
            SELECT id, cultureid, testmethodid, antibioticid, guidelinesid, dosage, specialconsiderationid,
                   ismanuallyset, setby, setat, cannedcommentid, freetextcomment, overriddenfromsusceptibilityid,
                   lastmodifieddate
            FROM astsusceptibilityoverride
            WHERE cultureid = @CultureId";

        var rows = await connect.QueryAsync<AstSusceptibilityOverrideDataModel>(sql, new { CultureId = cultureId });
        return rows.ToList();
    }

    /// <inheritdoc />
    public async Task SyncForCultureAsync(int cultureId, IEnumerable<AstSusceptibilityOverrideDataModel> overrides, string username)
    {
        var overrideList = overrides?.Where(o => o != null && o.IsManuallySet == "Yes").ToList()
                           ?? new List<AstSusceptibilityOverrideDataModel>();

        await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        await connect.OpenAsync();

        const string upsertSql = @"
            INSERT INTO astsusceptibilityoverride (
                cultureid, testmethodid, antibioticid, guidelinesid, dosage, specialconsiderationid,
                ismanuallyset, setby, setat, cannedcommentid, freetextcomment, overriddenfromsusceptibilityid, lastmodifieddate)
            VALUES (
                @CultureId, @TestMethodId, @AntibioticId, @GuidelinesId, @Dosage, @SpecialConsiderationId,
                @IsManuallySet, @SetBy, @SetAt, @CannedCommentId, @FreeTextComment, @OverriddenFromSusceptibilityId, now())
            ON CONFLICT (cultureid, testmethodid, antibioticid, guidelinesid, dosage, specialconsiderationid)
            DO UPDATE SET
                ismanuallyset = EXCLUDED.ismanuallyset,
                setby = EXCLUDED.setby,
                setat = EXCLUDED.setat,
                cannedcommentid = EXCLUDED.cannedcommentid,
                freetextcomment = EXCLUDED.freetextcomment,
                overriddenfromsusceptibilityid = EXCLUDED.overriddenfromsusceptibilityid,
                lastmodifieddate = now()";

        foreach (var row in overrideList)
        {
            row.CultureId = cultureId;
            if (string.IsNullOrEmpty(row.SetBy))
            {
                row.SetBy = username;
            }

            await connect.ExecuteAsync(upsertSql, row);
        }

        var keepKeys = overrideList
            .Select(o => AstSusceptibilityOverrideExtensions.BuildSusceptibilityLineKey(
                o.TestMethodId, o.AntibioticId, o.GuidelinesId, o.Dosage, o.SpecialConsiderationId))
            .Where(k => k.Length > 0)
            .ToHashSet();

        var existing = (await GetByCultureIdAsync(cultureId)).ToList();
        var deleted = 0;
        foreach (var existingRow in existing)
        {
            var key = AstSusceptibilityOverrideExtensions.BuildSusceptibilityLineKey(
                existingRow.TestMethodId,
                existingRow.AntibioticId,
                existingRow.GuidelinesId,
                existingRow.Dosage,
                existingRow.SpecialConsiderationId);

            if (keepKeys.Contains(key))
            {
                continue;
            }

            const string deleteSql = @"
                DELETE FROM astsusceptibilityoverride
                WHERE cultureid = @CultureId
                  AND testmethodid = @TestMethodId
                  AND antibioticid = @AntibioticId
                  AND guidelinesid = @GuidelinesId
                  AND dosage = @Dosage
                  AND specialconsiderationid = @SpecialConsiderationId";

            await connect.ExecuteAsync(deleteSql, existingRow);
            deleted++;
        }

        _logger.LogInformation(
            "AstSusceptibilityOverride sync: CultureId={CultureId}, Upserted={Upserted}, Deleted={Deleted}",
            cultureId, overrideList.Count, deleted);
    }

    /// <inheritdoc />
    public async Task DeleteByLineKeyAsync(int cultureId, string lineKey)
    {
        if (string.IsNullOrEmpty(lineKey))
        {
            return;
        }

        var parts = lineKey.Split('|');
        if (parts.Length != 5)
        {
            return;
        }

        await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        const string sql = @"
            DELETE FROM astsusceptibilityoverride
            WHERE cultureid = @CultureId
              AND testmethodid = @TestMethodId
              AND antibioticid = @AntibioticId
              AND guidelinesid = @GuidelinesId
              AND dosage = @Dosage
              AND specialconsiderationid = @SpecialConsiderationId";

        await connect.ExecuteAsync(sql, new
        {
            CultureId = cultureId,
            TestMethodId = int.Parse(parts[0]),
            AntibioticId = int.Parse(parts[1]),
            GuidelinesId = int.Parse(parts[2]),
            Dosage = int.Parse(parts[3]),
            SpecialConsiderationId = int.Parse(parts[4]),
        });
    }
}
