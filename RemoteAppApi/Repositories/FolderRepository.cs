using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public FolderRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<Folder> AddFolder(Folder folder)
        {

            var addedEntity = await _appDbContext.Folders.AddAsync(folder);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteFolder(int folderId)
        {
            var foundFolder = await _appDbContext.Folders.FirstOrDefaultAsync(e => e.FolderId == folderId);
            if (foundFolder == null) return;

            _appDbContext.Folders.Remove(foundFolder);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Folder>> GetAllFolders()
        {
            return await this._appDbContext.Folders.ToListAsync();
        }

        public  async Task<Folder> GetFolderById(int FolderId)
        {
            return await _appDbContext.Folders.FirstOrDefaultAsync(c => c.FolderId == FolderId);
        }

        public async Task<Folder> UpdateFolder(Folder folder)
        {
            var foundFolder = await _appDbContext.Folders.FirstOrDefaultAsync(e => e.FolderId == folder.FolderId);

            if (foundFolder != null)
            {
                foundFolder.FolderName = folder.FolderName;
                foundFolder.FolderDescription = folder.FolderDescription;
                foundFolder.ParentFolderId = folder.ParentFolderId;
                foundFolder.FolderStatus = folder.FolderStatus;
               

                await _appDbContext.SaveChangesAsync();

                return foundFolder;
            }

            return null;
        }
    }
}
