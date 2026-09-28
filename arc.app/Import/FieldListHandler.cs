using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Import
{
    public class FieldListHandler : IFieldListHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILanguageHandler _languageHandler;

        public FieldListHandler(IServiceProvider serviceProvider, ILanguageHandler languageHandler)
        {
            _serviceProvider = serviceProvider;
            _languageHandler = languageHandler;

        }

        public async Task<List<OptionsConfig>> GetFieldsAsync(TokenInfoModel token, string form)
        {
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();

            var formConfig = await formConfigDefinition.LoadFormAsync(form);

            var tableName = form.ToLower() == "createspecimenreceivedform" ? "specimen" : "";
            var fieldList = formConfig.GetFieldsForForm(tableName);

            var newOptionList = fieldList.FindAll(a => !string.IsNullOrWhiteSpace(a.Label))
                               .Select(f => new OptionsConfig
                               {
                                   Key = f.Id,
                                   Text = _languageHandler.TranslateAsync(f.Label, token.LanguageId).Result
                               }).ToList().OrderBy(f => f.Text);
            return newOptionList.ToList();
        }
    }
}
