using Drive2.StorageManagement;

namespace Drive2.Services.DirectoryService
{
    public interface IDirectoryService
    {
        List<EntryStruct> GetEntries(string path);
    }
}
