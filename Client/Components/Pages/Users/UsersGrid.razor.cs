using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RemoteAppWeb.Helpers;


using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RemoteAppWeb.Services;
using Microsoft.JSInterop;
using Action = Shared.Dto.Action;
using Microsoft.AspNetCore.Http;

using System.Reflection;
using System.ComponentModel.DataAnnotations;
using RemoteAppWeb.Services.Contracts;
using DevExpress.Blazor;
using Shared.Dto;

namespace RemoteApp.Pages.Users
{
    public partial class UsersGrid
    {

      
        [Inject]
        private IUserDataService UserService { get; set; }
        [Inject]
        private IRoleDataService RoleService { get; set; }

        int selectedvalue { get; set; }

        public List<UserDto> Userslist { get; set; }
        public List<RoleDto> Roleslist { get; set; }
        public int userId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public UserDto user { get; set; }

        public string newPassword { get; set; }
        public string confirmedPassword { get; set; }

        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> UserStatuses { get; set; }
       /* protected  async override void OnInitialized()
        {
           
        }*/

        public List<KeyValuePair<string, int>> GetEnumList<T>()
        {
            var list = new List<KeyValuePair<string, int>>();
            string display = null;
            foreach (UserStatus e in Enum.GetValues(typeof(T)))
            {
                display = e.GetType()
                 .GetMember(e.ToString())
                 .FirstOrDefault()?
                 .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();
                list.Add(new KeyValuePair<string, int>(display, (int)e));
            }
            return list;
        }

        public async Task RefreshGrid()
        {
            //OnInitialized();
            CanRead = true;// CheckPermissionService.CheckAutorisation(Action.Lecture, Resource.Folders);
            CanCreate = true;//CheckPermissionService.CheckAutorisation(Action.Creation, Resource.Folders);
            CanUpdate = true;//CheckPermissionService.CheckAutorisation(Action.Modification, Resource.Folders);
            CanDelete = true; //CheckPermissionService.CheckAutorisation(Action.Suppression, Resource.Folders);

            if (CanRead)
            {
                Userslist = (List<UserDto>)await UserService.GetAllUsers();
                Roleslist = (List<RoleDto>)await RoleService.GetRolesList();
                UserStatuses = GetEnumList<UserStatus>();
                //base.OnInitialized();
            }
          
        }
        async Task OnRowUpdating(UserDto user, Dictionary<string, object> newValue)
        {
            user.PasswordChanged = false;
            user.UserPassword = "string";
            SetNewValues(user, newValue);

            Console.WriteLine($"The update user : {user}");
            Console.WriteLine($"The updated usr Id: {user.UserId}");
            Console.WriteLine($"The updated user Login: {user.UserLogin}");
         
            await  UserService.UpdateUser(user);
            // OnInitialized();
            RefreshGrid();
            //StateHasChanged();
        }
        async Task OnRowRemoving(UserDto x)
        {
            
          
               await  UserService.DeleteUser(x.UserId);
               // OnInitialized();
               RefreshGrid();
               // StateHasChanged();

         
        }
        void OnRowInserting(Dictionary<string, object> newValue)
        {
            var user = new UserDto();
          
            user.UserPassword = Hlp.GetSha1((string)newValue["UserNom"], "QSDFGHJKLM@&987654321", "pass");
            SetNewValues(user, newValue);
         
            Console.WriteLine($"The new user : {user}");
            Console.WriteLine($"The new usr Id: {user.UserId}");
            Console.WriteLine($"The new user Login: {user.UserLogin}");


            UserService.AddUser(user);
            RefreshGrid();
            //StateHasChanged();
        }
        public void ClosePopup()
        {
            RefreshGrid();
            PopupVisible = false;
        }
        public void ChangePassword(UserDto user)
        {
            PopupVisible = true;
            userId = user.UserId;
        }
        public void SaveNewPassword()
        {
            if (newPassword == confirmedPassword)
            {
                UserService.SaveNewPassword(userId, newPassword);
            }
        }
        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            var model = e.DataItem as UserDto;
            if (model == null)
            {
                model = new UserDto();

            }

            e.EditModel = model;
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var entry = (UserDto)e.EditModel;
            if (e.IsNew)
            {

                UserService.AddUser(entry);
            }

            else
                UserService.UpdateUser(entry);

            RefreshGrid();
           // StateHasChanged();

        }
        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var entry = (UserDto)e.DataItem;
         
        
                UserService.DeleteUser(entry.UserId);
                RefreshGrid();
           
 
        }
        private void SetNewValues(UserDto user, Dictionary<string, object> newValue)
        {
            user.PasswordChanged = false;
          
            //user.Role = null;
            Console.WriteLine($"The dictionnary : {newValue}");
            foreach (var kvp in newValue)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            foreach (var field in newValue.Keys)
            {
                switch (field)
                {
                    case "UserId":
                        user.UserId = (int)newValue[field];
                        break;
                    case "UserNom":
                        user.UserNom = (string)newValue[field];
                        break;

                    case "UserPrenom":
                        user.UserPrenom = (string)newValue[field];
                        break;

                    case "UserPhone":
                        user.UserPhone = (string)newValue[field];
                        break;


                    case "UserEmail":
                        user.UserEmail = (string)newValue[field];
                        break;

                    case "UserMaxCapacity":
                        user.UserMaxCapacity = Convert.ToInt32(newValue[field]);
                        break;

                    case "RoleId":
                        var selectedPosition = (int)newValue[field];
                        Console.WriteLine($"The selected pos : {selectedPosition}");
                        user.RoleId = Roleslist[selectedPosition - 1].RoleId; // Subtract 1 because positions are 1-based.
                        break;

                    case "UserLogin":
                        user.UserLogin = (string)newValue[field];
                        break;

                    case "UserStatusInt":
                        user.UserStatus = (UserStatus)newValue[field];
                        break;


                }
            }

            user.RoleId = selectedvalue; //Roleslist[selectedvalue].RoleId;
            Console.WriteLine($"The Selectd value: {selectedvalue}");
            Console.WriteLine($"The List Role 0 Id : {Roleslist[0].RoleId}");
            Console.WriteLine($"The List Role 1  Id : {Roleslist[1].RoleId}");
            Console.WriteLine($"The List Role 2  Id : {Roleslist[2].RoleId}");
            Console.WriteLine($"The User Role Id : {user.RoleId}");
        }

    }
}
