using arc.app.Common;
using arc.app.Exports;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace arc.api.Services;

/// <summary>
/// Background service that checks enabled export schedules every minute and runs exports when due.
/// Due-ness is evaluated against the API server's local time (<see cref="DateTime.Now"/>) so the
/// time of day a user enters (and sees in the schedules list) is the local wall-clock time the export runs.
/// </summary>
public class ExportScheduleBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ExportScheduleBackgroundService> _logger;
    private readonly TimeSpan _checkInterval;

    /// <summary>
    /// Token used for scheduled exports (no user context - runs without lab/org restrictions).
    /// </summary>
    private static readonly TokenInfoModel SystemToken = new()
    {
        Id = "scheduled-export",
        Username = "scheduled-export",
        LaboratoryId = "",
        OrganisationId = "",
        LanguageId = "1"
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportScheduleBackgroundService"/> class.
    /// </summary>
    public ExportScheduleBackgroundService(
        IServiceScopeFactory scopeFactory,
        IHostEnvironment environment,
        ILogger<ExportScheduleBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _checkInterval = environment.IsDevelopment() ? TimeSpan.FromSeconds(15) : TimeSpan.FromMinutes(1);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var logWriter = scope.ServiceProvider.GetRequiredService<ILogWriter>();
            logWriter.LogInfo("Export schedule background service started", nameof(ExportScheduleBackgroundService), nameof(ExecuteAsync));
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndRunSchedulesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                using var errorScope = _scopeFactory.CreateScope();
                var errorLog = errorScope.ServiceProvider.GetRequiredService<ILogWriter>();
                errorLog.LogError($"Export schedule check failed: {ex.Message}", nameof(ExportScheduleBackgroundService), nameof(ExecuteAsync));
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task CheckAndRunSchedulesAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var scheduleRepository = scope.ServiceProvider.GetRequiredService<IExportScheduleRepository>();
        var exportRunHandler = scope.ServiceProvider.GetRequiredService<IExportRunHandler>();
        var logWriter = scope.ServiceProvider.GetRequiredService<ILogWriter>();

        var schedules = await scheduleRepository.GetEnabledSchedulesAsync();
        if (schedules.Count == 0)
        {
            return;
        }

        var now = DateTime.Now;
        logWriter.LogInfo(
            $"Checking {schedules.Count} enabled export schedule(s) at {now:yyyy-MM-dd HH:mm:ss} local " +
            $"({DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC, timezone={TimeZoneInfo.Local.Id}, offset={TimeZoneInfo.Local.GetUtcOffset(now)})",
            nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));

        foreach (var schedule in schedules)
        {
            if (stoppingToken.IsCancellationRequested) break;

            if (!IsDue(schedule, now))
            {
                logWriter.LogInfo(
                    $"Export schedule not due: id={schedule.Id}, name={schedule.Name}, frequency={schedule.Frequency}, " +
                    $"timeOfDay={(schedule.TimeOfDay.HasValue ? schedule.TimeOfDay.Value.ToString() : "(none)")}, " +
                    $"dayOfMonth={(schedule.DayOfMonth.HasValue ? schedule.DayOfMonth.Value.ToString() : "(none)")}, " +
                    $"localNow={now:yyyy-MM-dd HH:mm:ss}",
                    nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));
                continue;
            }

            logWriter.LogInfo($"Export schedule triggered: id={schedule.Id}, name={schedule.Name}, profileId={schedule.ExportProfileId}", nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));

            try
            {
                var queryFilters = BuildQueryFiltersFromSchedule(schedule, logWriter);
                logWriter.LogInfo($"Running export for schedule id={schedule.Id}, profileId={schedule.ExportProfileId}, frequency={schedule.Frequency}, changesToInclude={schedule.ChangesToInclude ?? "(none)"}", nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));
                await exportRunHandler.RunExportAsync(queryFilters, SystemToken);
                logWriter.LogInfo($"Export completed for schedule id={schedule.Id}", nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));
            }
            catch (Exception ex)
            {
                logWriter.LogError($"Export failed for schedule id={schedule.Id}: {ex.Message}", nameof(ExportScheduleBackgroundService), nameof(CheckAndRunSchedulesAsync));
            }
        }
    }

    /// <summary>
    /// Determines if a schedule is due to run at the given server-local time.
    /// </summary>
    /// <param name="schedule">The enabled export schedule to evaluate.</param>
    /// <param name="now">The current server-local time (<see cref="DateTime.Now"/>).</param>
    /// <returns><c>true</c> when the schedule should run now; otherwise <c>false</c>.</returns>
    private static bool IsDue(ExportScheduleModel schedule, DateTime now)
    {
        return schedule.Frequency switch
        {
            "1526" => now.Minute < 2, // hourly - run in first 2 minutes of each hour
            "1527" => schedule.TimeOfDay.HasValue && IsDailyDue(schedule.TimeOfDay.Value, now),
            "1528" => schedule.TimeOfDay.HasValue && schedule.DayOfMonth.HasValue && IsMonthlyDue(schedule.TimeOfDay.Value, schedule.DayOfMonth.Value, now),
            _ => false
        };
    }

    /// <summary>
    /// Determines whether a daily schedule is due, treating <paramref name="timeOfDay"/> as a
    /// server-local wall-clock time and allowing a +/-2 minute window around it.
    /// </summary>
    /// <param name="timeOfDay">The configured local time of day for the run.</param>
    /// <param name="now">The current server-local time (<see cref="DateTime.Now"/>).</param>
    /// <returns><c>true</c> when the current local time falls within the run window; otherwise <c>false</c>.</returns>
    private static bool IsDailyDue(TimeSpan timeOfDay, DateTime now)
    {
        var scheduledTime = now.Date.Add(timeOfDay);
        var windowStart = scheduledTime.AddMinutes(-2);
        var windowEnd = scheduledTime.AddMinutes(2);
        var nowTime = now;
        return nowTime >= windowStart && nowTime <= windowEnd;
    }

    /// <summary>
    /// Determines whether a monthly schedule is due, treating <paramref name="timeOfDay"/> as a
    /// server-local wall-clock time on the configured day of month (clamped to the last day of the
    /// current month) and allowing a +/-2 minute window around it.
    /// </summary>
    /// <param name="timeOfDay">The configured local time of day for the run.</param>
    /// <param name="dayOfMonth">The configured day of month (1-31); clamped to the month's last day.</param>
    /// <param name="now">The current server-local time (<see cref="DateTime.Now"/>).</param>
    /// <returns><c>true</c> when the current local date and time fall within the run window; otherwise <c>false</c>.</returns>
    private static bool IsMonthlyDue(TimeSpan timeOfDay, int dayOfMonth, DateTime now)
    {
        var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
        var effectiveDay = Math.Min(dayOfMonth, lastDayOfMonth);
        var scheduledDate = new DateTime(now.Year, now.Month, effectiveDay).Add(timeOfDay);
        var windowStart = scheduledDate.AddMinutes(-2);
        var windowEnd = scheduledDate.AddMinutes(2);
        return now >= windowStart && now <= windowEnd;
    }

    /// <summary>
    /// Builds query filters from the schedule's filter JSON and adds exportprofileid, exportScheduleId, changesToInclude, outputDirectory.
    /// </summary>
    /// <param name="schedule">The enabled export schedule due to run.</param>
    /// <param name="logWriter">Logger used when filter JSON cannot be parsed.</param>
    private static QueryFilterConfig BuildQueryFiltersFromSchedule(ExportScheduleModel schedule, ILogWriter logWriter)
    {
        var filters = new QueryFilterConfig();
        filters.AddInteger("exportprofileid", schedule.ExportProfileId);
        filters.AddInteger("exportScheduleId", schedule.Id);
        var changesToInclude = !string.IsNullOrWhiteSpace(schedule.ChangesToInclude)
            ? schedule.ChangesToInclude
            : (schedule.IncrementalOnly ? "newandmodified" : null);
        filters.AddString("changesToInclude", changesToInclude ?? "");
        if (!string.IsNullOrWhiteSpace(schedule.OutputDirectory))
        {
            filters.AddString("outputDirectory", schedule.OutputDirectory);
        }

        if (!string.IsNullOrWhiteSpace(schedule.Filter))
        {
            try
            {
                var filter = JObject.Parse(schedule.Filter);
                foreach (var prop in filter.Properties())
                {
                    var key = ExportScheduleFilterBuilder.MapFilterKeyForRunQuery(prop.Name.ToLowerInvariant());
                    var value = prop.Value?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        filters.AddString(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter.LogError($"Failed to parse export schedule filter JSON for schedule id={schedule.Id}: {ex.Message}", nameof(ExportScheduleBackgroundService), nameof(BuildQueryFiltersFromSchedule));
            }
        }

        return filters;
    }
}
