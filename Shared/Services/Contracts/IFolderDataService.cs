

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IFolderDataService
    {
        Task<IEnumerable<FolderDto>> GetAllFolders();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<FolderDto> AddFolder(FolderDto folder);
        Task UpdateFolder(FolderDto folder);
        Task DeleteFolder(int folderId);
        Task<FolderDto> GetFolder(int folderId);
    }
}
