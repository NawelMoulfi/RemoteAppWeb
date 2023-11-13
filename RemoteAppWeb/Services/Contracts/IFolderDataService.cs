using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IFolderDataService
    {
        Task<IEnumerable<Folder>> GetAllFolders();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<Folder> AddFolder(Folder folder);
        Task UpdateFolder(Folder folder);
        Task DeleteFolder(int folderId);
        Task<Folder> GetFolder(int folderId);
    }
}
