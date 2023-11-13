using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public RoleRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Role> AddRole(Role role)
        {
            var addedEntity = await _appDbContext.Roles.AddAsync(role);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteRole(int roleId)
        {
            var foundRole = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == roleId );///s.FirstOrDefaultAsyn(e => e.RoleId == roleId);
            if (foundRole == null) return;

            _appDbContext.Roles.Remove(foundRole);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
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

            return roles;
         
        }
    

        public  async Task<Role> GetRoleById(int RoleId)
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
            return role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            var foundRole = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == role.RoleId);

            if (foundRole != null)
            {
               /* foundClient.FirstName = client.FirstName;
                foundClient.LastName = client.LastName;
                foundClient.PhoneNumber = client.PhoneNumber;
                foundClient.Adresse = client.Adresse;
                foundClient.PID = client.PID;
                foundClient.Wilaya = client.Wilaya;*/
               foundRole.RoleName = role.RoleName;
              // foundRole.ModuleActionRoles = role.ModuleActionRoles;
               // foundRole.ModuleActions = role.ModuleActions;


                await _appDbContext.SaveChangesAsync();

                return foundRole;
            }

            return null;
        }
    }
}

