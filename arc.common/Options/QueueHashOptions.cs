namespace arc.common.Options;

/// <summary>
/// Configuration options for the Queue table hash chain.
/// </summary>
public class QueueHashOptions
{
    /// <summary>
    /// Seed used for the first record in the hash chain. If null or empty, defaults to "ARC-QUEUE-CHAIN-SEED".
    /// Changing this value invalidates all existing hash chains - use only for new deployments.
    /// </summary>
    public string ChainSeed { get; set; }
}
