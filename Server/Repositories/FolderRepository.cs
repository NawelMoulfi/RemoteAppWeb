using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public FolderRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<FolderDto> AddFolder(FolderDto folder)
        {
            var Folder = mapper.Map<FolderDto, Folder>(folder);
            var addedEntity = await _appDbContext.Folders.AddAsync(Folder);
            await _appDbContext.SaveChangesAsync();
            var FolderDTO = mapper.Map<Folder, FolderDto>(addedEntity.Entity);
            return FolderDTO;
        }

        public async Task DeleteFolder(int folderId)
        {
            var foundFolder = await _appDbContext.Folders.Include(p => p.ParentFolder).FirstOrDefaultAsync(e => e.FolderId == folderId);
            if (foundFolder == null) return;

            _appDbContext.Folders.Remove(foundFolder);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<FolderDto>> GetAllFolders()
        {
          
            var Folders = new List<Folder>();
            Folders = await this._appDbContext.Folders.ToListAsync();
            List<FolderDto> FolderDtos = new List<FolderDto>();
            foreach (var folder in Folders)
            {
                FolderDtos.Add(mapper.Map<Folder,FolderDto>(folder));
            }
            return FolderDtos;
        }

        public  async Task<FolderDto> GetFolderById(int FolderId)
        {
            
            var Folder = await _appDbContext.Folders.FirstOrDefaultAsync(c => c.FolderId == FolderId);
            var FolderDTO = mapper.Map<Folder, FolderDto>(Folder);
            return FolderDTO;
        }

        public async Task<FolderDto> UpdateFolder(FolderDto folder)
        {
            var Folder = mapper.Map<FolderDto, Folder>(folder);
            var foundFolder = await _appDbContext.Folders.FirstOrDefaultAsync(e => e.FolderId == Folder.FolderId);

            if (foundFolder != null)
            {
                foundFolder.FolderName = Folder.FolderName;
                foundFolder.FolderDescription = Folder.FolderDescription;
                foundFolder.ParentFolderId = Folder.ParentFolderId;
                foundFolder.FolderStatus = Folder.FolderStatus;
               

                await _appDbContext.SaveChangesAsync();

                var FolderDTO = mapper.Map<Folder, FolderDto>(foundFolder);
                return FolderDTO;
            }

            return null;
        }
    }
}
