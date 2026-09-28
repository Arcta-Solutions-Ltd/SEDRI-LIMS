using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Alert;
using arc.domain.Alert;
using System.Linq;

namespace arc.app.Alert
{
    /// <summary>
    /// Maps the AlertDetailsModel type onto the AlertDetails type.
    /// </summary>
    public class AlertDetailsModelToAlertDetailsMapper : IMapType<AlertDetailsModel, AlertDetails>
    {
        /// <summary>
        /// Maps a <see cref="AlertDetailsModel"/> object to a <see cref="AlertDetails"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="AlertDetailsModel"/> object to map.</param>
        /// <returns>The mapped <see cref="AlertDetails"/> object.</returns>
        public AlertDetails Map(AlertDetailsModel source)
        {
            var result = source.Map<AlertDetails>();
            source.TestGrid ??= [];
            source.SusceptibilityGrid ??= [];

            result.TestGrid = source.TestGrid.Select(t => TestGridToTestGridModelMapper(t)).ToList();
            result.SusceptibilityGrid = source.SusceptibilityGrid.Select(s => SusceptibilityGridToSusceptibilityGridModelMapper(s)).ToList();
            return result;
        }

        /// <summary>
        /// Maps a <see cref="TestGridModel"/> object to a <see cref="TestGrid"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="TestGridModel"/> object to map.</param>
        /// <returns>The mapped <see cref="TestGrid"/> object.</returns>
        private static TestGrid TestGridToTestGridModelMapper(TestGridModel grid)
        {
            return new TestGrid
            {
                TestName = grid.Test,
                FieldName = grid.Field,
                Comparison = grid.Comparison,
                CompValue = grid.ListValue ?? grid.NumberValue ?? grid.StringValue
            };
        }

        /// <summary>
        /// Maps a <see cref="SusceptibilityGridModel"/> object to a <see cref="SusceptibilityGrid"/> object.
        /// </summary>
        /// <param name="grid">The <see cref="SusceptibilityGridModel"/> object to map.</param>
        /// <returns>The mapped <see cref="SusceptibilityGrid"/> object.</returns>
        private static SusceptibilityGrid SusceptibilityGridToSusceptibilityGridModelMapper(SusceptibilityGridModel grid)
        {
            return new SusceptibilityGrid { AntibioticId = grid.AntibioticId, SusceptibilityId = grid.SusceptibilityId };
        }
    }
}
