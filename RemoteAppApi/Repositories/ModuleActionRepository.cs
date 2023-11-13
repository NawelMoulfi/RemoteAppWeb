using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using System.Reflection;

namespace RemoteAppApi.Repositories
{
    public class ModuleActionRepository : IModuleActionRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public ModuleActionRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ModuleAction> AddModuleAction(ModuleAction moduleaction)
        {

            var addedEntity = await _appDbContext.ModuleActions.AddAsync(moduleaction);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteModuleAction(int moduleactionId)
        {
            var foundModuleAction = await _appDbContext.ModuleActions.FirstOrDefaultAsync(e => e.ModuleActionID == moduleactionId);
            if (foundModuleAction == null) return;

            _appDbContext.ModuleActions.Remove(foundModuleAction);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModuleAction>> GetAllModulActions()
        {
            return await this._appDbContext.ModuleActions.Include(p => p.Module).ToListAsync();
        }

        public async Task<ModuleAction> GetModuleActionAById(int ModuleActionId)
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).FirstOrDefaultAsync(c => c.ModuleActionID == ModuleActionId);
        }

        public async Task<ModuleAction> UpdateModuleAction(ModuleAction moduleaction)
        {
            var foundModelAction = await _appDbContext.ModuleActions.FirstOrDefaultAsync(e => e.ModuleActionID == moduleaction.ModuleActionID);

            if (foundModelAction != null)
            {
                foundModelAction.ModuleID = moduleaction.ModuleID;
                foundModelAction.Resource = moduleaction.Resource;
                foundModelAction.Action = moduleaction.Action;
                foundModelAction.ModuleActionNom = moduleaction.ModuleActionNom;
                await _appDbContext.SaveChangesAsync();

                return foundModelAction;
            }

            return null;
        }

        public async Task<IEnumerable<ModuleAction>> GetResourceAction(Resource resource, RemoteApp.Data.Models.Action action)
        {
            var resourceCondition = _appDbContext.ModuleActions.Include(p => p.Module).Where(e => e.Resource == resource);
            var actionCondition = resourceCondition.Where(e => e.Action == action);
            var filteredModuleActions = await actionCondition.ToListAsync();

            //return filteredModuleActions;
            return resourceCondition;
        }

        public async Task<IEnumerable<ModuleAction>> GetListModuleActionsByResource(Resource resource)
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => ma.Resource == resource).ToListAsync();
        }
        public async Task<IEnumerable<RemoteApp.Data.Models.Action>> GetActionsByResource(Resource resource)
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => ma.Resource == resource).Select(column => column.Action).ToListAsync();
        }
        public async Task<IEnumerable<ModuleAction>> GetActionsByResourceExcludingFirst()
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => (int)ma.Resource > 0).ToListAsync();
        }
        public async Task<IEnumerable<Resource>> GetUsedResources()
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Select(ma => ma.Resource).Distinct().ToListAsync();
        }
    }
}
