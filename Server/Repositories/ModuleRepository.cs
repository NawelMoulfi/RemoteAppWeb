using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public ModuleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ModuleDto> AddModule(ModuleDto module)
        {
            var Module = mapper.Map<ModuleDto, Module>(module);
            var addedEntity = await _appDbContext.Module.AddAsync(Module);
            await _appDbContext.SaveChangesAsync();
            var ModuleDTO = mapper.Map<Module, ModuleDto>(addedEntity.Entity);
            return ModuleDTO;
            
        }

        public async Task DeleteModule(int moduleId)
        {
            var foundModule = await _appDbContext.Module.FirstOrDefaultAsync(e => e.ModuleID == moduleId);
            if (foundModule == null) return;

            _appDbContext.Module.Remove(foundModule);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModuls()
        {
            var Modules = new List<Module>();
            Modules = await this._appDbContext.Module.ToListAsync();
            List<ModuleDto> ModuleDtos = new List<ModuleDto>();
            foreach (var Module in Modules)
            {
                ModuleDtos.Add(mapper.Map<Module, ModuleDto>(Module));
            }
            return ModuleDtos;
           
        }

        public async Task<ModuleDto> GetModuleById(int ModuleId)
        {
            var Module = await _appDbContext.Module.FirstOrDefaultAsync(c => c.ModuleID == ModuleId);
            var ModuleDTO = mapper.Map<Module, ModuleDto>(Module);
            return ModuleDTO;

           
        }

        public async  Task<ModuleDto> UpdateModule(ModuleDto module)
        {
            var Module = mapper.Map<ModuleDto, Module>(module);
            var foundModel = await _appDbContext.Module.FirstOrDefaultAsync(e => e.ModuleID == Module.ModuleID);

            if (foundModel != null)
            {
                foundModel.ModuleGroup = module.ModuleGroup;
                foundModel.ModuleNom = module.ModuleNom;
                await _appDbContext.SaveChangesAsync();

                var ModuleDTO = mapper.Map<Module, ModuleDto>(foundModel);

                return ModuleDTO;
            }

            return null;
        }
    }
}
