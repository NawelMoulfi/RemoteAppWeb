using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class ModuleActionRoleRepository : IModuleActionRoleRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public ModuleActionRoleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<ModuleActionRole> AddModuleActionRole(ModuleActionRole moduleactionrole)
        {
            var addedEntity = await _appDbContext.ModuleActionRoles.AddAsync(moduleactionrole);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async  Task DeleteModuleActionRole(int moduleactionroleId)
        {
            var foundModuleActionRole = await _appDbContext.ModuleActionRoles.FirstOrDefaultAsync(e => e.ModuleActionRoleId == moduleactionroleId);
            if (foundModuleActionRole == null) return;

            _appDbContext.ModuleActionRoles.Remove(foundModuleActionRole);
            await _appDbContext.SaveChangesAsync();
        }

        public  async Task<IEnumerable<ModuleActionRole>> GetAllModulActionRoles()
        {
            return await this._appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).ToListAsync();
        }

        public async  Task<ModuleActionRole> GetModuleActionRoleById(int ModuleActionRoleId)
        {
            return await _appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).FirstOrDefaultAsync(c => c.ModuleActionRoleId == ModuleActionRoleId);
        }
        public async Task<ModuleActionRole> GetModuleActionRoleByRoleAndModuleActionId(int RoleId, int ModuleActionId)
        {
            return await _appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).FirstOrDefaultAsync(c => c.RoleId== RoleId && c.ModuleActionId == ModuleActionId);

        }

        public async Task<ModuleActionRole> UpdateModuleActionRole(ModuleActionRole moduleactionrole)
        {
            var foundModelActionRole = await _appDbContext.ModuleActionRoles.FirstOrDefaultAsync(e => e.ModuleActionRoleId == moduleactionrole.ModuleActionRoleId);

            if (foundModelActionRole != null)
            {
                foundModelActionRole.RoleId = moduleactionrole.RoleId;
                foundModelActionRole.ModuleActionId = moduleactionrole.ModuleActionId;
                await _appDbContext.SaveChangesAsync();

                return foundModelActionRole;
            }

            return null;
        }
    }
}
