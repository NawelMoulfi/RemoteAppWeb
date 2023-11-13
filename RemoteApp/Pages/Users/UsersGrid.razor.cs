using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RemoteApp.Helpers;

using RemoteApp.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RemoteApp.Services;
using Microsoft.JSInterop;
using Action = RemoteApp.Data.Models.Action;
using Microsoft.AspNetCore.Http;
using RemoteApp.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using RemoteApp.Services.Contracts;
using DevExpress.Blazor;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;

namespace RemoteApp.Pages.Users
{
    public partial class UsersGrid
    {

      
        [Inject]
        private IUserDataService UserService { get; set; }
        [Inject]
        private IRoleDataService RoleService { get; set; }

        int selectedvalue { get; set; }

        public List<User> Userslist { get; set; }
        public List<Role> Roleslist { get; set; }
        public int userId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public User user { get; set; }

        public string newPassword { get; set; }
        public string confirmedPassword { get; set; }

        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> UserStatuses { get; set; }
        protected  async override void OnInitialized()
        {
            CanRead = true;// CheckPermissionService.CheckAutorisation(Action.Lecture, Resource.Folders);
            CanCreate = true;//CheckPermissionService.CheckAutorisation(Action.Creation, Resource.Folders);
            CanUpdate = true;//CheckPermissionService.CheckAutorisation(Action.Modification, Resource.Folders);
            CanDelete = true; //CheckPermissionService.CheckAutorisation(Action.Suppression, Resource.Folders);

            if (CanRead)
            {
                Userslist = (List<User>)await UserService.GetAllUsers();
                Roleslist = (List<Role>)await RoleService.GetRolesList();
                UserStatuses = GetEnumList<UserStatus>();
                //base.OnInitialized();
            }
            else
            {
                JSRuntime.InvokeAsync<object>("open", "/NoAccess/", "_self");
            }
        }

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

        public void RefreshGrid()
        {
            OnInitialized();
        }
        async Task OnRowUpdating(User user, Dictionary<string, object> newValue)
        {
            user.PasswordChanged = false;
            user.UserPassword = "string";
            SetNewValues(user, newValue);

            Console.WriteLine($"The update user : {user}");
            Console.WriteLine($"The updated usr Id: {user.UserId}");
            Console.WriteLine($"The updated user Login: {user.UserLogin}");
         
            await  UserService.UpdateUser(user);
            OnInitialized();
            StateHasChanged();
        }
        async Task OnRowRemoving(User x)
        {
            try
            {
               await  UserService.DeleteUser(x.UserId);
                OnInitialized();
                StateHasChanged();

            }
            catch (Exception e)
            {

                OnInitialized();
                JSRuntime.InvokeAsync<object>("alert", "vous ne pouvez pas supprimer cet utilisateur !");
                // Console.WriteLine(e);
                // throw;
            }
        }
        void OnRowInserting(Dictionary<string, object> newValue)
        {
            var user = new User();
          
            user.UserPassword = Hlp.GetSha1((string)newValue["UserNom"], "QSDFGHJKLM@&987654321", "pass");
            SetNewValues(user, newValue);
         
            Console.WriteLine($"The new user : {user}");
            Console.WriteLine($"The new usr Id: {user.UserId}");
            Console.WriteLine($"The new user Login: {user.UserLogin}");


            UserService.AddUser(user);
            OnInitialized();
            StateHasChanged();
        }
        public void ClosePopup()
        {
            OnInitialized();
            PopupVisible = false;
        }
        public void ChangePassword(User user)
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
            var model = e.DataItem as User;
            if (model == null)
            {
                model = new User();

            }

            e.EditModel = model;
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var entry = (User)e.EditModel;
            if (e.IsNew)
            {

                UserService.AddUser(entry);
            }

            else
                UserService.UpdateUser(entry);

            OnInitialized();
            StateHasChanged();

        }
        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var entry = (User)e.DataItem;
            try
            {
                UserService.DeleteUser(entry.UserId);
                OnInitialized();
                StateHasChanged();

            }
            catch (Exception ex)
            {

                OnInitialized();
                JSRuntime.InvokeAsync<object>("alert", "vous ne pouvez pas supprimer cet element !");
                // Console.WriteLine(e);
                // throw;
            }
        }
        private void SetNewValues(User user, Dictionary<string, object> newValue)
        {
            user.PasswordChanged = false;
          
            user.Role = null;
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
