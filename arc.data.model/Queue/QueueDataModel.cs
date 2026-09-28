namespace arc.data.model.Queue;

/// <summary>
/// Represents the fields in the queue table in the database.
/// </summary>
public class QueueDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the record ID associated with the queue item.
    /// </summary>
    public int? RecordId { get; set; }

    /// <summary>
    /// Gets or sets the message for the queue item.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the username associated with the queue item.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Gets or sets the date and time the queue item was added.
    /// </summary>
    public DateTime Added { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the topic table to the queue table.
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the event table to the queue table.
    /// </summary>
    public int? EventId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the queue table. This links to the eventstatus list in the listitem table.
    /// </summary>
    public int? EventStatusId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the queue table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the patient table to the queue table.
    /// </summary>
    public int? PatientId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the queue table. This links to the state list in the listitem table.
    /// </summary>
    public int? StateId { get; set; }

    /// <summary>
    /// Gets or sets any error message associated with the queue item.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets the SHA256 hash chain value for tamper detection.
    /// The hash is computed from the previous record's Message and the current record's Message (by Id order).
    /// </summary>
    public string? Hash { get; set; }
}
