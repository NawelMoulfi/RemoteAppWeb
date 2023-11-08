using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using System;
using System.Text;
using System.Security.Cryptography;
using Azure;

namespace RemoteAppApi.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public UserRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this._appDbContext.Users.Include(p => p.Role).ToListAsync();//.ToListAsync();
        }

        public async Task<User> GetUserById(int UserId)
        {
            return await _appDbContext.Users.Include(p => p.Role).FirstOrDefaultAsync(c => c.UserId == UserId);
        }

        public async Task<User> AddUser(User user)
        {
            var addedEntity = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<User> UpdateUser(User user)
        {
            var foundUser = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (foundUser != null)
            {
                foundUser.UserNom = user.UserNom;
                foundUser.UserPrenom = user.UserPrenom;
                foundUser.UserPhone = user.UserPhone;
                foundUser.UserEmail = user.UserEmail;
                foundUser.UserLogin = user.UserLogin;
                foundUser.UserStatus= user.UserStatus;
                foundUser.RoleId = user.RoleId;
                foundUser.UserMaxCapacity = user.UserMaxCapacity;
               // foundUser.Role = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == user.RoleId);

                await _appDbContext.SaveChangesAsync();

                return foundUser;
            }

            return null;
        }

        public async Task DeleteUser(int userId)
        {
            var foundUser = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == userId);
            if (foundUser == null) return;

            _appDbContext.Users.Remove(foundUser);
            await _appDbContext.SaveChangesAsync();

        }
        public async Task<User> FindUserByCredentials(string username, string password)
        {
            // Compute the hash of the provided password
            var passwordHash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(username + "QSDFGHJKLM@&987654321" + password));
            var passwordHashString = passwordHash.Aggregate("", (current, item) => current + item);

            Console.WriteLine($" The password  that was passed : {password}");
            Console.WriteLine($" passwordHashString: {passwordHashString}");
            Console.WriteLine($" The user name that was passed : {username}");
            // Query the database for a user with the provided username and computed password hash
            var user = await _appDbContext.Users
      .Include(u => u.Role) // Include the Role
      .FirstOrDefaultAsync(e => e.UserLogin == username);

            // Check if the user and the associated role are found
            if (user != null && user.Role != null)
            {
                var roleId = user.RoleId; // Retrieve the RoleId from the user
                var moduleActions = _appDbContext.ModuleActionRoles
    .Where(mar => mar.RoleId == roleId) // Filter by the user's role name
    .Join(
        _appDbContext.ModuleActions, // Join with the ModuleAction table
        mar => mar.ModuleActionId, // Join on ModuleActionId from ModuleActionRole
        ma => ma.ModuleActionID, // Join on ModuleActionId from ModuleAction
        (mar, ma) => ma // Select the ModuleAction
    )
    .Include(ma => ma.Module) // Include the Module information for each ModuleAction
    .ToList();
                if (moduleActions != null) {
                    user.Role.ModuleActions = moduleActions;
                }

              
              
            }
            Console.WriteLine($" Userlogin: {user.UserLogin}");
            if(user.UserPassword.Equals(passwordHashString)) return user;

            return null;
        }
        public async Task<User> FindUserByUID(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserLogin == username );

            return user;
        }

        public async  Task SaveNewPassword(int userId, string UserPassword)
      {
            var foundUser = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == userId);
            if (foundUser == null) return;
            var xx = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(foundUser.UserLogin + "QSDFGHJKLM@&987654321" + foundUser.UserPassword));
            foundUser.UserPassword  = xx.Aggregate("", (current, item) => current + item);
         
            foundUser.PasswordChanged = true;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
