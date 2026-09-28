using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace arc.api.Services;

/// <summary>
/// Contract for throttling and rate limiting login attempts.
/// </summary>
public interface ILoginThrottleService
{
    /// <summary>
    /// Checks if the given IP address is currently rate limited based on recent activity.
    /// </summary>
    /// <param name="ipAddress">Remote IP address string.</param>
    /// <returns>True if the IP is rate limited; otherwise false.</returns>
    bool IsIpRateLimited(string ipAddress);

    /// <summary>
    /// Checks whether a user has exceeded the maximum allowed failed login attempts in the window.
    /// </summary>
    /// <param name="userName">The username being evaluated.</param>
    /// <returns>True if throttled; otherwise false.</returns>
    bool IsUserThrottled(string userName);

    /// <summary>
    /// Records the outcome of a login attempt for the specified user and IP.
    /// </summary>
    /// <param name="userName">The username that attempted login.</param>
    /// <param name="ipAddress">The originating IP address.</param>
    /// <param name="succeeded">True if the attempt succeeded; false if it failed.</param>
    void RecordLoginAttempt(string userName, string ipAddress, bool succeeded);
}

/// <summary>
/// Default in-memory implementation of <see cref="ILoginThrottleService"/>.
/// Tracks failed login counts per user and request rates per IP using <see cref="IMemoryCache"/>.
/// </summary>
public class LoginThrottleService : ILoginThrottleService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<LoginThrottleService> _logger;

    // Defaults; move to options if/when needed.
    private const int MaxFailedAttemptsPerUser = 5;
    private static readonly TimeSpan FailedAttemptWindow = TimeSpan.FromMinutes(15);

    private const int MaxRequestsPerIpWindow = 60; // total login hits per window
    private static readonly TimeSpan IpWindow = TimeSpan.FromMinutes(5);

    private const int MinSecondsBetweenAttemptsPerIp = 1; // simple burst limiter

    /// <summary>
    /// Creates a new instance of the throttle service.
    /// </summary>
    public LoginThrottleService(IMemoryCache cache, ILogger<LoginThrottleService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsIpRateLimited(string ipAddress)
    {
        var now = DateTime.UtcNow;

        var countKey = $"ip:{ipAddress}:count";
        var lastKey = $"ip:{ipAddress}:last";

        // Basic burst control
        var last = _cache.Get<DateTime?>(lastKey);
        if (last.HasValue && (now - last.Value).TotalSeconds < MinSecondsBetweenAttemptsPerIp)
        {
            return true;
        }

        // Sliding window counter for the IP
        var record = _cache.Get<(int count, DateTime windowStart)?>(countKey);
        if (record is null || (now - record.Value.windowStart) > IpWindow)
        {
            _cache.Set(countKey, (1, now), now.Add(IpWindow));
        }
        else
        {
            var newCount = record.Value.count + 1;
            _cache.Set(countKey, (newCount, record.Value.windowStart), record.Value.windowStart.Add(IpWindow));
            if (newCount > MaxRequestsPerIpWindow)
            {
                return true;
            }
        }

        _cache.Set(lastKey, now, now.Add(IpWindow));
        return false;
    }

    /// <inheritdoc />
    public bool IsUserThrottled(string userName)
    {
        var key = $"user:{userName}:fails";
        var record = _cache.Get<(int count, DateTime windowStart)?>(key);
        if (record is null)
        {
            return false;
        }
        if ((DateTime.UtcNow - record.Value.windowStart) > FailedAttemptWindow)
        {
            return false;
        }
        return record.Value.count >= MaxFailedAttemptsPerUser;
    }

    /// <inheritdoc />
    public void RecordLoginAttempt(string userName, string ipAddress, bool succeeded)
    {
        if (succeeded)
        {
            // Reset user failure counter on success
            var key = $"user:{userName}:fails";
            _cache.Remove(key);
            return;
        }

        var failKey = $"user:{userName}:fails";
        var now = DateTime.UtcNow;
        var record = _cache.Get<(int count, DateTime windowStart)?>(failKey);
        if (record is null || (now - record.Value.windowStart) > FailedAttemptWindow)
        {
            _cache.Set(failKey, (1, now), now.Add(FailedAttemptWindow));
        }
        else
        {
            var newCount = record.Value.count + 1;
            _cache.Set(failKey, (newCount, record.Value.windowStart), record.Value.windowStart.Add(FailedAttemptWindow));
            if (newCount == MaxFailedAttemptsPerUser)
            {
                _logger.LogWarning("Excessive failed login attempts for user {user}", userName);
            }
        }
    }
}
