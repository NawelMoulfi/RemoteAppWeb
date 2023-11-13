using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public RoleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<RoleDto> AddRole(RoleDto role)
        {
            var Role = mapper.Map<RoleDto, Role>(role);
            var addedEntity = await _appDbContext.Roles.AddAsync(Role);
            await _appDbContext.SaveChangesAsync();
            var RoleDTO = mapper.Map<Role, RoleDto>(addedEntity.Entity);
            return RoleDTO;
            
        }

        public async Task DeleteRole(int roleId)
        {
            var foundRole = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == roleId );///s.FirstOrDefaultAsyn(e => e.RoleId == roleId);
            if (foundRole == null) return;

            _appDbContext.Roles.Remove(foundRole);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            var roles = await _appDbContext.Roles.ToListAsync();

            foreach (var role in roles)
            {
                role.ModuleActions = _appDbContext.ModuleActionRoles
                    .Where(mar => mar.RoleId == role.RoleId)
                    .Join(
                        _appDbContext.ModuleActions,
                        mar => mar.ModuleActionId,
                        ma => ma.ModuleActionID,
                        (mar, ma) => ma
                    )
                    .ToList();
            }
            List<RoleDto> RoleDtos = new List<RoleDto>();
            foreach (var Role in roles)
            {
                RoleDtos.Add(mapper.Map<Role, RoleDto>(Role));
            }
            return RoleDtos;

           
         
        }
    

        public  async Task<RoleDto> GetRoleById(int RoleId)
        {
            var role = await _appDbContext.Roles.FirstOrDefaultAsync(c => c.RoleId == RoleId);
            role.ModuleActions = _appDbContext.ModuleActionRoles
                  .Where(mar => mar.RoleId == role.RoleId)
                  .Join(
                      _appDbContext.ModuleActions,
                      mar => mar.ModuleActionId,
                      ma => ma.ModuleActionID,
                      (mar, ma) => ma
                  )
                  .ToList();
            var RoleDTO = mapper.Map<Role, RoleDto>(role);
            return RoleDTO;
          
        }

        public async Task<RoleDto> UpdateRole(RoleDto role)
        {
            var Role = mapper.Map<RoleDto, Role>(role);
            var foundRole = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == role.RoleId);

            if (foundRole != null)
            {
               
               foundRole.RoleName = Role.RoleName;
             


                await _appDbContext.SaveChangesAsync();
                var RoleDTO = mapper.Map<Role, RoleDto>(foundRole);

                return RoleDTO;

                
            }

            return null;
        }
    }
}

