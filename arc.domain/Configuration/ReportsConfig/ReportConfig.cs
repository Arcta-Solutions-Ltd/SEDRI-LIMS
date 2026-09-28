using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig
{
    /// <summary>
    /// Represents the configuration for a report.
    /// </summary>
    public class ReportConfig
    {
        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title of the report.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the header of the report.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the footer of the report.
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether alerts are included in the report.
        /// </summary>
        public bool IncludeAlerts { get; set; }

        /// <summary>
        /// Gets or sets the configurable parameter of the report.
        /// </summary>
        public string Configurable { get; set; }

        /// <summary>
        /// Gets or sets the section sources of the report.
        /// </summary>
        public List<SectionSource> SectionSource { get; set; }

        /// <summary>
        /// Gets or sets the main sections of the report.
        /// </summary>
        public List<string> MainSections { get; set; }

        /// <summary>
        /// Gets or sets the organism sections of the report.
        /// </summary>
        public List<string> OrganismSections { get; set; }

        /// <summary>
        /// Gets or sets the final sections of the report.
        /// </summary>
        public List<string> FinalSections { get; set; }

        /// <summary>
        /// Adds a section to the main sections.
        /// </summary>
        /// <param name="section">The section to add.</param>
        public void AddMainSection(string section)
        {
            MainSections.Add(section.ToLower());
        }

        /// <summary>
        /// Adds a section to the organism sections.
        /// </summary>
        /// <param name="section">The section to add.</param>
        public void AddOrganismSection(string section)
        {
            OrganismSections.Add(section.ToLower());
        }

        /// <summary>
        /// Adds a section to the final sections.
        /// </summary>
        /// <param name="section">The section to add.</param>
        public void AddFinalSection(string section)
        {
            FinalSections.Add(section.ToLower());
        }

        /// <summary>
        /// Deletes a section from all section lists.
        /// </summary>
        /// <param name="section">The section to delete.</param>
        public void DeleteSection(string section)
        {
            MainSections.Remove(section.ToLower());
            OrganismSections.Remove(section.ToLower());
            FinalSections.Remove(section.ToLower());
        }

        /// <summary>
        /// Determines whether the specified section exists in any of the section lists.
        /// </summary>
        /// <param name="section">The section to check.</param>
        /// <returns>true if the section exists; otherwise, false.</returns>
        public bool ContainsSection(string section)
        {
            section = section.ToLower();
            return MainSections.Contains(section) || OrganismSections.Contains(section) || FinalSections.Contains(section);
        }

        /// <summary>
        /// Moves a section up in all section lists.
        /// </summary>
        /// <param name="section">The section to move.</param>
        public void MoveSectionUp(string section)
        {
            section = section.ToLower();
            MainSections = MoveUp(section, MainSections);
            OrganismSections = MoveUp(section, OrganismSections);
            FinalSections = MoveUp(section, FinalSections);
        }

        /// <summary>
        /// Moves a section down in all section lists.
        /// </summary>
        /// <param name="section">The section to move.</param>
        public void MoveSectionDown(string section)
        {
            section = section.ToLower();
            MainSections = MoveDown(section, MainSections);
            OrganismSections = MoveDown(section, OrganismSections);
            FinalSections = MoveDown(section, FinalSections);
        }

        /// <summary>
        /// Moves a section up in the specified list.
        /// </summary>
        /// <param name="section">The section to move.</param>
        /// <param name="sections">The list of sections.</param>
        /// <returns>The modified list of sections.</returns>
        private List<string> MoveUp(string section, List<string> sections)
        {
            int index = sections.IndexOf(section);
            if (index > 0)
            {
                sections.RemoveAt(index);
                sections.Insert(index - 1, section);
            }
            return sections;
        }

        /// <summary>
        /// Moves a section down in the specified list.
        /// </summary>
        /// <param name="section">The section to move.</param>
        /// <param name="sections">The list of sections.</param>
        /// <returns>The modified list of sections.</returns>
        private List<string> MoveDown(string section, List<string> sections)
        {
            int index = sections.IndexOf(section);
            if (index >= 0 && index < sections.Count - 1)
            {
                sections.RemoveAt(index);
                sections.Insert(index + 1, section);
            }
            return sections;
        }
    }
}
