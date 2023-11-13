using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;
using System.Reflection;

namespace RemoteAppApi.Repositories
{
    public class ModuleActionRepository : IModuleActionRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public ModuleActionRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ModuleActionDto> AddModuleAction(ModuleActionDto moduleaction)
        {
            var ModuleAction = mapper.Map<ModuleActionDto, ModuleAction>(moduleaction);
            var addedEntity = await _appDbContext.ModuleActions.AddAsync(ModuleAction);
            await _appDbContext.SaveChangesAsync();
            var ModuleActionDTO = mapper.Map<ModuleAction, ModuleActionDto>(addedEntity.Entity);
            return ModuleActionDTO;
      
        }

        public async Task DeleteModuleAction(int moduleactionId)
        {
            var foundModuleAction = await _appDbContext.ModuleActions.FirstOrDefaultAsync(e => e.ModuleActionID == moduleactionId);
            if (foundModuleAction == null) return;

            _appDbContext.ModuleActions.Remove(foundModuleAction);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModuleActionDto>> GetAllModulActions()
        {
            var ModuleActions = new List<ModuleAction>();
            ModuleActions = await this._appDbContext.ModuleActions.Include(p => p.Module).ToListAsync();
            List<ModuleActionDto> ModuleActionDtos = new List<ModuleActionDto>();
            foreach (var ModuleAction in ModuleActions)
            {
                ModuleActionDtos.Add(mapper.Map<ModuleAction, ModuleActionDto>(ModuleAction));
            }
            return ModuleActionDtos;
            
        }

        public async Task<ModuleActionDto> GetModuleActionAById(int ModuleActionId)
        {
            var ModuleAction = await _appDbContext.ModuleActions.Include(p => p.Module).FirstOrDefaultAsync(c => c.ModuleActionID == ModuleActionId);
            var ModuleActionDTO = mapper.Map<ModuleAction, ModuleActionDto>(ModuleAction);
            return ModuleActionDTO;
           
        }

        public async Task<ModuleActionDto> UpdateModuleAction(ModuleActionDto moduleaction)
        {
            var ModelAction = mapper.Map<ModuleActionDto, ModuleAction>(moduleaction);
            var foundModelAction = await _appDbContext.ModuleActions.FirstOrDefaultAsync(e => e.ModuleActionID == ModelAction.ModuleActionID);

            if (foundModelAction != null)
            {
                foundModelAction.ModuleID = ModelAction.ModuleID;
                foundModelAction.Resource = ModelAction.Resource;
                foundModelAction.Action = ModelAction.Action;
                foundModelAction.ModuleActionNom = ModelAction.ModuleActionNom;
                await _appDbContext.SaveChangesAsync();
                var ModuleActionDTO = mapper.Map<ModuleAction, ModuleActionDto>(foundModelAction);
               
                return ModuleActionDTO;
            }

            return null;
        }

        public async Task<IEnumerable<ModuleActionDto>> GetResourceAction(Resource resource, Shared.Dto.Action action)
        {
            var resourceCondition = _appDbContext.ModuleActions.Include(p => p.Module).Where(e => e.Resource == resource);
            var actionCondition = resourceCondition.Where(e => e.Action == action);
            var filteredModuleActions = await actionCondition.ToListAsync();
            List<ModuleActionDto> ModuleActionDtos = new List<ModuleActionDto>();
            foreach (var ModuleAction in filteredModuleActions)
            {
                ModuleActionDtos.Add(mapper.Map<ModuleAction, ModuleActionDto>(ModuleAction));
            }
            return ModuleActionDtos;
        }

        public async Task<IEnumerable<ModuleActionDto>> GetListModuleActionsByResource(Resource resource)
        {
            var ModuleActions = new List<ModuleAction>();
            ModuleActions = await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => ma.Resource == resource).ToListAsync();
            List<ModuleActionDto> ModuleActionDtos = new List<ModuleActionDto>();
            foreach (var ModuleAction in ModuleActions)
            {
                ModuleActionDtos.Add(mapper.Map<ModuleAction, ModuleActionDto>(ModuleAction));
            }
            return ModuleActionDtos;
          
        }
        public async Task<IEnumerable<Shared.Dto.Action>> GetActionsByResource(Resource resource)
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => ma.Resource == resource).Select(column => column.Action).ToListAsync();
        }
        public async Task<IEnumerable<ModuleActionDto>> GetActionsByResourceExcludingFirst()
        {
            var ModuleActions = new List<ModuleAction>();
            ModuleActions = await _appDbContext.ModuleActions.Include(p => p.Module).Where(ma => (int)ma.Resource > 0).ToListAsync();
            List<ModuleActionDto> ModuleActionDtos = new List<ModuleActionDto>();
            foreach (var ModuleAction in ModuleActions)
            {
                ModuleActionDtos.Add(mapper.Map<ModuleAction, ModuleActionDto>(ModuleAction));
            }
            return ModuleActionDtos;
         
        }
        public async Task<IEnumerable<Resource>> GetUsedResources()
        {
            return await _appDbContext.ModuleActions.Include(p => p.Module).Select(ma => ma.Resource).Distinct().ToListAsync();
        }
    }
}
