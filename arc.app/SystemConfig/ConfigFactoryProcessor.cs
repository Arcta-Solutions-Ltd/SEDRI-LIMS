using arc.app.Config.Forms;
using arc.app.Config.Reports;
using arc.app.Config.Reports.DataSection;
using arc.common.Models.SystemConfig;
using Newtonsoft.Json;

namespace arc.app.SystemConfig
{
    public class ConfigFactoryProcessor : IConfigFactoryProcessor
    {
        private IFormConfigFactory _formConfigFactory;
        private IReportFactory _reportFactory;
        private IDataSectionFactory _dataSectionFactory;

        public ConfigFactoryProcessor(IFormConfigFactory formConfigFactory, IReportFactory reportFactory, IDataSectionFactory dataSectionFactory)
        {
            _formConfigFactory = formConfigFactory;
            _reportFactory = reportFactory;
            _dataSectionFactory = dataSectionFactory;
        }

        public string GetInternalConfig(ConfigsModel model)
        {
            var configContents = "";

            if (model.Type != null)
            {
                switch (model.Type.ToLower())
                {
                    case "datasource":
                        var sourceDef = _dataSectionFactory.GetSection(model.ConfigName);
                        configContents = JsonConvert.SerializeObject(sourceDef);
                        break;
                    case "form":
                        var formDef = _formConfigFactory.GetForm(model.ConfigName);
                        configContents = JsonConvert.SerializeObject(formDef);
                        break;
                    case "page":
                        break;
                    case "report":
                        configContents = _reportFactory.GetReport(model.ConfigName).Get();
                        break;
                }
            }

            return configContents;
        }
    }
}
