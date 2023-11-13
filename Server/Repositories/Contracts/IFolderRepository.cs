using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IFolderRepository
    {
        Task<IEnumerable<FolderDto>> GetAllFolders();
        Task<FolderDto> GetFolderById(int FolderId);
        Task<FolderDto> AddFolder(FolderDto folder);
        // Task<Event> AddEvent(Event event);
        Task<FolderDto> UpdateFolder(FolderDto folder);
        Task DeleteFolder(int folderId);
    }
}
