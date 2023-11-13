using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllFolders();
        Task<Folder> GetFolderById(int FolderId);
        Task<Folder> AddFolder(Folder folder);
        // Task<Event> AddEvent(Event event);
        Task<Folder> UpdateFolder(Folder folder);
        Task DeleteFolder(int folderId);
    }
}
