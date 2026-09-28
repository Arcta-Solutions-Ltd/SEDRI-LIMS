using System;
using System.Security.Cryptography;
using System.Text;
using arc.common.Options;
using Microsoft.Extensions.Options;

namespace arc.app.Common
{
    /// <summary>
    /// Implementation of <see cref="IQueueHashService"/> using SHA256 for the Queue table hash chain.
    /// Uses a configurable seed for the first record in the chain (from QueueHash:ChainSeed in appsettings.json).
    /// </summary>
    public class QueueHashService : IQueueHashService
    {
        private const string DefaultChainSeed = "ARC-QUEUE-CHAIN-SEED";
        private readonly IOptionsMonitor<QueueHashOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueHashService"/> class.
        /// </summary>
        /// <param name="options">Configuration options for the queue hash chain.</param>
        public QueueHashService(IOptionsMonitor<QueueHashOptions> options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public string ComputeHash(string previousMessage, string currentMessage)
        {
            var seed = string.IsNullOrEmpty(_options.CurrentValue?.ChainSeed)
                ? DefaultChainSeed
                : _options.CurrentValue.ChainSeed;
            var previous = string.IsNullOrEmpty(previousMessage) ? seed : previousMessage;
            var input = previous + currentMessage;
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(bytes);
            var sb = new StringBuilder(64);
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <inheritdoc />
        public bool ValidateHash(string previousMessage, string currentMessage, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }
            var expectedHash = ComputeHash(previousMessage, currentMessage);
            return string.Equals(expectedHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
