using System.Collections.Generic;

namespace arc.common.Models.Export
{
    public interface IWhonetAntibioticColumns
    {
        //void AddOrganismRecord(string specimenId, string Organism, List<string> antibioticRows);
        void AddOrganismRecord(int cultureId, List<WhonetAntibiotic> antibioticRows);
        //List<string> AddIntoGrid(List<string> lines);
        List<string> GetAntibioticList();
        List<WNExportList> GetAntibioticForCulture(int cultureId);
    }
}
