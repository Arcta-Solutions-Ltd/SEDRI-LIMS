using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Factory class for creating section format configurations based on the definition name.
/// </summary>
public class SectionFormatFactory : ISectionFormatFactory, IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a section format configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the section format definition.</param>
    /// <returns>An instance of a section format configuration, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "doublecolumnone" => new DoubleColumnOneConfig(),
            "doublecolumntwo" => new DoubleColumnTwoConfig(),
            "doublecolumnthree" => new DoubleColumnThreeConfig(),
            "doublecolumnfour" => new DoubleColumnFourConfig(),
            "doublecolumnwithgridone" => new DoubleColumnWithGridOneConfig(),
            "doublecolumnwithgridtwo" => new DoubleColumnWithGridTwoConfig(),
            "doublecolumnwithtwogrids" => new DoubleColumnWithTwoGridsConfig(),
            "singlecolumnone" => new SingleColumnOneConfig(),
            "singlecolumntwo" => new SingleColumnTwoConfig(),
            "tableone" => new TableOneConfig(),
            "tablesinglecolumn" => new TableSingleColumnConfig(),
            "tabletwo" => new TableTwoConfig(),
            "dynamicsinglecolumnone" => new DynamicSingleColumnOneConfig(),
            "imageone" => new ImageOneConfig(),
            "absolute" => new AbsoluteConfig(),
            _ => null,
        };
    }
}
