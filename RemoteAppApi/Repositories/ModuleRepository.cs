using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public ModuleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Module> AddModule(Module module)
        {
            var addedEntity = await _appDbContext.Module.AddAsync(module);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteModule(int moduleId)
        {
            var foundModule = await _appDbContext.Module.FirstOrDefaultAsync(e => e.ModuleID == moduleId);
            if (foundModule == null) return;

            _appDbContext.Module.Remove(foundModule);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Module>> GetAllModuls()
        {
            return await this._appDbContext.Module.ToListAsync();//.ToListAsync();
        }

        public async Task<Module> GetModuleById(int ModuleId)
        {
            return await _appDbContext.Module.FirstOrDefaultAsync(c => c.ModuleID == ModuleId);
        }

        public async  Task<Module> UpdateModule(Module module)
        {
            var foundModel = await _appDbContext.Module.FirstOrDefaultAsync(e => e.ModuleID == module.ModuleID);

            if (foundModel != null)
            {
                foundModel.ModuleGroup = module.ModuleGroup;
                foundModel.ModuleNom = module.ModuleNom;
                await _appDbContext.SaveChangesAsync();

                return foundModel;
            }

            return null;
        }
    }
}
