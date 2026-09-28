using arc.app.Common;
using arc.common;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports;

/// <summary>
/// Event handler for adding an export schedule.
/// Builds the Filter JSON from form fields and validates unique name per profile.
/// </summary>
internal class AddExportScheduleEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExportScheduleEvent"/> class.
    /// </summary>
    public AddExportScheduleEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var repository = _serviceProvider.GetService<IExportScheduleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        var vm = JsonConvert.DeserializeObject<ExportScheduleFormViewModel>(dataToSave);
        if (vm == null)
        {
            throw new ArgumentException("Invalid export schedule data", nameof(dataToSave));
        }

        var model = BuildExportScheduleModel(vm, logWriter);
        model.Enabled = string.IsNullOrWhiteSpace(vm.Enabled)
            || string.Equals(vm.Enabled, "Yes", StringComparison.OrdinalIgnoreCase);

        var newId = await repository.AddExportScheduleAsync(JsonConvert.SerializeObject(model, new JsonBooleanConverter()));
        logWriter?.LogInfo($"Export schedule created: id={newId}, name={model.Name}, profileId={model.ExportProfileId}", nameof(AddExportScheduleEvent), nameof(RunAsync));
        return newId;
    }

    private static ExportScheduleModel BuildExportScheduleModel(ExportScheduleFormViewModel vm, ILogWriter logWriter)
    {
        var filter = ExportScheduleFilterBuilder.BuildFilterFromViewModel(vm);
        ExportScheduleFilterBuilder.LogMultiValueFilterCriteria(logWriter, filter, vm.Name ?? string.Empty, nameof(AddExportScheduleEvent));

        TimeSpan? timeOfDay = null;
        if (!string.IsNullOrWhiteSpace(vm.TimeOfDay) && TimeSpan.TryParse(vm.TimeOfDay, out var ts))
        {
            timeOfDay = ts;
        }

        var changesToInclude = MapChangesToIncludeFromForm(vm.ChangesToInclude, vm.IncrementalOnly);

        return new ExportScheduleModel
        {
            ExportProfileId = vm.ExportProfileId,
            Name = vm.Name?.Trim() ?? string.Empty,
            Filter = filter.Count > 0 ? filter.ToString() : null,
            Frequency = vm.Frequency ?? "1527",
            TimeOfDay = timeOfDay,
            DayOfMonth = vm.DayOfMonth,
            IncrementalOnly = string.Equals(changesToInclude, "newandmodified", StringComparison.OrdinalIgnoreCase) || string.Equals(vm.IncrementalOnly, "Yes", StringComparison.OrdinalIgnoreCase),
            OutputDirectory = string.IsNullOrWhiteSpace(vm.OutputDirectory) ? null : vm.OutputDirectory.Trim(),
            ChangesToInclude = changesToInclude
        };
    }

    private static string? MapChangesToIncludeFromForm(string? formValue, string? incrementalOnly)
    {
        if (!string.IsNullOrWhiteSpace(formValue))
        {
            return formValue.Trim() switch
            {
                "1529" => "newonly",
                "1530" => "newandmodified",
                "New only" => "newonly",
                "New and modified" => "newandmodified",
                "newonly" => "newonly",
                "newandmodified" => "newandmodified",
                _ => null
            };
        }
        return string.Equals(incrementalOnly, "Yes", StringComparison.OrdinalIgnoreCase) ? "newandmodified" : null;
    }
}
