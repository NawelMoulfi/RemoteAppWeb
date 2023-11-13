using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class ModuleActionRoleRepository : IModuleActionRoleRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public ModuleActionRoleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public  async Task<ModuleActionRoleDto> AddModuleActionRole(ModuleActionRoleDto moduleactionrole)
        {
            var ModuleActionRole = mapper.Map<ModuleActionRoleDto, ModuleActionRole>(moduleactionrole);
            var addedEntity = await _appDbContext.ModuleActionRoles.AddAsync(ModuleActionRole);
            await _appDbContext.SaveChangesAsync();
            var ModuleActionRoleDTO = mapper.Map<ModuleActionRole, ModuleActionRoleDto>(addedEntity.Entity);
            return ModuleActionRoleDTO;
        }

        public async  Task DeleteModuleActionRole(int moduleactionroleId)
        {
            var foundModuleActionRole = await _appDbContext.ModuleActionRoles.FirstOrDefaultAsync(e => e.ModuleActionRoleId == moduleactionroleId);
            if (foundModuleActionRole == null) return;

            _appDbContext.ModuleActionRoles.Remove(foundModuleActionRole);
            await _appDbContext.SaveChangesAsync();
        }

        public  async Task<IEnumerable<ModuleActionRoleDto>> GetAllModulActionRoles()
        {
            var ModuleActionRoles = new List<ModuleActionRole>();
            ModuleActionRoles = await this._appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).ToListAsync();
            List<ModuleActionRoleDto> ModuleActionRoleDtos = new List<ModuleActionRoleDto>();
            foreach (var ModuleActionRole in ModuleActionRoles)
            {
                ModuleActionRoleDtos.Add(mapper.Map<ModuleActionRole, ModuleActionRoleDto>(ModuleActionRole));
            }
            return ModuleActionRoleDtos;
           
        }

        public async  Task<ModuleActionRoleDto> GetModuleActionRoleById(int ModuleActionRoleId)
        {
            var ModuleActionRole = await _appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).FirstOrDefaultAsync(c => c.ModuleActionRoleId == ModuleActionRoleId);
            var ModuleActionRoleDTO = mapper.Map<ModuleActionRole, ModuleActionRoleDto>(ModuleActionRole);
            return ModuleActionRoleDTO;

            
        }
        public async Task<ModuleActionRoleDto> GetModuleActionRoleByRoleAndModuleActionId(int RoleId, int ModuleActionId)
        {
            var ModuleActionRole = await _appDbContext.ModuleActionRoles.Include(p => p.Role).Include(p => p.ModuleAction).FirstOrDefaultAsync(c => c.RoleId == RoleId && c.ModuleActionId == ModuleActionId);
            var ModuleActionRoleDTO = mapper.Map<ModuleActionRole, ModuleActionRoleDto>(ModuleActionRole);
            return ModuleActionRoleDTO;
          

        }

        public async Task<ModuleActionRoleDto> UpdateModuleActionRole(ModuleActionRoleDto moduleactionrole)
        {
            var ModelActionRole = mapper.Map<ModuleActionRoleDto, ModuleActionRole>(moduleactionrole);
            var foundModelActionRole = await _appDbContext.ModuleActionRoles.FirstOrDefaultAsync(e => e.ModuleActionRoleId == moduleactionrole.ModuleActionRoleId);

            if (foundModelActionRole != null)
            {
                foundModelActionRole.RoleId = ModelActionRole.RoleId;
                foundModelActionRole.ModuleActionId = ModelActionRole.ModuleActionId;
                await _appDbContext.SaveChangesAsync();

                var ModuleActionRoleDTO = mapper.Map<ModuleActionRole, ModuleActionRoleDto>(foundModelActionRole);

                return ModuleActionRoleDTO;
            }

            return null;
        }
    }
}
