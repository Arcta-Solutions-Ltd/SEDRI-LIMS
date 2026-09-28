namespace arc.app.Configuration
{
    public interface IFieldTypeList
    {
        int GetIdFromName(string name);
        bool TryGetIdFromName(string name, out int id);
        string GetNameFromId(int id);
        int GetGridIdFromName(string name);
        string GetGridNameFromId(int id);
    }
}
