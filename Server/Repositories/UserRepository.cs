using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using System;
using System.Text;
using System.Security.Cryptography;
using Azure;
using AutoMapper;
using Server;
using Shared.Dto;
using System.Data;

namespace RemoteAppApi.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public UserRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var Users = new List<User>();
            Users = await this._appDbContext.Users.Include(p => p.Role).ToListAsync();//.ToListAsync();

            List<UserDto> UserDtos = new List<UserDto>();
            foreach (var User in Users)
            {
                UserDtos.Add(mapper.Map<User, UserDto>(User));
            }
            return UserDtos;

           
        }

        public async Task<UserDto> GetUserById(int UserId)
        {
            var User = await _appDbContext.Users.Include(p => p.Role).FirstOrDefaultAsync(c => c.UserId == UserId);
            var UserDTO = mapper.Map<User, UserDto>(User);
            return UserDTO;
            
        }

        public async Task<UserDto> AddUser(UserDto user)
        {
            var User = mapper.Map<UserDto, User>(user);
            var addedEntity = await _appDbContext.Users.AddAsync(User);
            await _appDbContext.SaveChangesAsync();
            var UserDTO = mapper.Map<User, UserDto>(addedEntity.Entity);
            return UserDTO;
            
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            var User = mapper.Map<UserDto, User>(user);
            var foundUser = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == User.UserId);

            if (foundUser != null)
            {
                foundUser.UserNom = User.UserNom;
                foundUser.UserPrenom = User.UserPrenom;
                foundUser.UserPhone = User.UserPhone;
                foundUser.UserEmail = User.UserEmail;
                foundUser.UserLogin = User.UserLogin;
                foundUser.UserStatus= User.UserStatus;
                foundUser.RoleId = user.RoleId;
                foundUser.UserMaxCapacity = user.UserMaxCapacity;
               // foundUser.Role = await _appDbContext.Roles.FirstOrDefaultAsync(e => e.RoleId == user.RoleId);

                await _appDbContext.SaveChangesAsync();
                var UserDTO = mapper.Map<User, UserDto>(foundUser);

                return UserDTO;
               
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
        public async Task<UserDto> FindUserByCredentials(string username, string password)
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
            var UserDTO = mapper.Map<User, UserDto>(user);

          
            if (user.UserPassword.Equals(passwordHashString)) return UserDTO;

            return null;
        }
        public async Task<UserDto> FindUserByUID(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.UserLogin == username );
            var UserDTO = mapper.Map<User, UserDto>(user);

            return UserDTO;
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
