using arc.app.Common;
using arc.common.Models.Specimen;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Inserts a new culture for the specimen and type with the next unique culture number and parent self-reference.
/// Specimens may have multiple cultures of the same type (e.g. manual instrument requests each need a distinct isolate row).
/// <para>
/// Culture numbers are allocated by atomically incrementing the <c>NextCultureNumber</c> watermark on the
/// <c>Specimen</c> row rather than computing <c>MAX(culturenumber)+1</c> from live rows. This ensures a
/// number is never reused after the previously highest-numbered isolate has been deleted.
/// </para>
/// </summary>
internal class CreateCultureForSpecimenAndTypeCommand : ICommandWithTypeReturningInteger<CreateCultureForSpecimenAndTypeModel>
{
    /// <summary>
    /// Inserts a new <c>Culture</c> row for the given specimen and culture type, allocates a unique
    /// culture number, and sets the self-referencing <c>ParentCultureId</c>.
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/> used for all database operations.</param>
    /// <param name="model">
    /// The model containing <c>SpecimenId</c> (the owning specimen) and <c>TypeId</c> (the culture type to create).
    /// </param>
    /// <param name="logWriter">Optional log writer; when provided, an info entry is written on success.</param>
    /// <returns>The primary key (<c>Id</c>) of the newly inserted <c>Culture</c> row.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, CreateCultureForSpecimenAndTypeModel model, ILogWriter logWriter = null)
    {
        var nextNum = await connect.QueryFirstAsync<int>(
            @"UPDATE Specimen
              SET    NextCultureNumber = NextCultureNumber + 1
              WHERE  Id = @SpecimenId
              RETURNING NextCultureNumber;",
            new { model.SpecimenId });

        const string insertSql = @"insert into culture(specimenid, typeid, culturenumber, displayonreport, moredata, lastmodifieddate)
                    values(@SpecimenId, @TypeId, @CultureNumber, 'Yes', '{}'::jsonb, now()) returning id";
        var newId = await connect.QueryFirstAsync<int>(insertSql,
            new { model.SpecimenId, TypeId = model.TypeId, CultureNumber = nextNum });

        await connect.ExecuteAsync(
            "update culture set ParentCultureId = @Id where Id = @Id and ParentCultureId is null",
            new { Id = newId });

        logWriter?.LogInfo(
            $"CreateCultureForSpecimenAndTypeAsync: created culture id={newId} specimenId={model.SpecimenId} typeId={model.TypeId}",
            nameof(CreateCultureForSpecimenAndTypeCommand),
            nameof(ExecuteAsync));
        return newId;
    }
}
