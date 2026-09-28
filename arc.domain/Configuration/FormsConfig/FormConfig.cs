using System.Collections.Generic;

namespace arc.domain.Configuration.FormsConfig
{
    public class FormConfig
    {
        public string Name { get; set; }
        public string Formtype { get; set; }
        public string View { get; set; }
        public bool NewItem { get; set; }
        public string SingleItemName { get; set; }
        public string Title { get; set; }
        public string InitialQuery { get; set; }
        public string SaveEvent { get; set; }
        public string FinishButtonText { get; set; }
        public bool Collapsible { get; set; }
        public bool Expanded { get; set; }
        public bool SuppressRecordView { get; set; }
        public string RecordView { get; set; }
        public string StartState { get; set; }
        public string DisplaySettings { get; set; }
        public bool UseListData { get; set; }
        public string UIEvent { get; set; }
        public string DataSection { get; set; }
        public string Configurable { get; set; }
        public string DefaultView { get; set; }
        public List<string> ConfigureActions { get; set; }
        public List<string> Pages { get; set; }
        public List<FormRulesConfig> Rules { get; set; }
        public string SaveOperation { get; set; }

        /// <summary>
        /// Set when the form can be run again from part way through after a save, which is how several
        /// specimens are registered against one request. Null on a form that saves once and closes.
        /// </summary>
        public FormRepeatConfig Repeat { get; set; }

    }
}
