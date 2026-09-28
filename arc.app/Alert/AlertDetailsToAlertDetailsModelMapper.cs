using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Alert;
using arc.domain.Alert;
using System.Linq;

namespace arc.app.Alert
{
    /// <summary>
    /// Maps the AlertDetails type onto the AlertDetailsModel type.
    /// </summary>
    public class AlertDetailsToAlertDetailsModelMapper : IMapType<AlertDetails, AlertDetailsModel>
    {
        /// <summary>
        /// Maps a <see cref="AlertDetails"/> object to a <see cref="AlertDetailsModel"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="AlertDetails"/> object to map.</param>
        /// <returns>The mapped <see cref="AlertDetailsModel"/> object.</returns>
        public AlertDetailsModel Map(AlertDetails source)
        {
            var result = source.Map<AlertDetailsModel>();
            result.TestGrid = source.TestGrid.Select(t => TestGridToTestGridModelMapper(t)).ToList();
            result.SusceptibilityGrid = source.SusceptibilityGrid.Select(s => SusceptibilityGridToSusceptibilityGridModelMapper(s)).ToList();
            return result;
        }

        /// <summary>
        /// Maps a <see cref="TestGrid"/> object to a <see cref="TestGridModel"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="TestGrid"/> object to map.</param>
        /// <returns>The mapped <see cref="TestGridModel"/> object.</returns>
        private static TestGridModel TestGridToTestGridModelMapper(TestGrid grid)
        {
            return new TestGridModel { Test = grid.TestName, Field = grid.FieldName, Comparison = grid.Comparison, ListValue = grid.CompValue, StringValue = grid.CompValue, NumberValue = grid.CompValue, Matched = false };
        }

        /// <summary>
        /// Maps a <see cref="SusceptibilityGrid"/> object to a <see cref="SusceptibilityGridModel"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="SusceptibilityGrid"/> object to map.</param>
        /// <returns>The mapped <see cref="SusceptibilityGridModel"/> object.</returns>
        private static SusceptibilityGridModel SusceptibilityGridToSusceptibilityGridModelMapper(SusceptibilityGrid grid)
        {
            return new SusceptibilityGridModel { AntibioticId = grid.AntibioticId, SusceptibilityId = grid.SusceptibilityId };
        }

    }
}



